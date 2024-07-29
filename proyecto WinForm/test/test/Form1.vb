Imports System.Net.Http
Imports System.ServiceModel
Imports System.Text.Json
Imports Newtonsoft.Json
Imports test.ClienteMl
Imports test.Producto
Imports test.WSProductos
Imports test.WSClientes
Imports test.WSorden
Imports System.Globalization
Imports System.Net

Public Class Form1

    Dim dtSucs As DataTable

    'Datos ML
    Dim apiMLBase As String = "https://api.mercadolibre.com/"
    Dim userId As Integer = 257128833
    Dim clientId As String = "5038796649356510"
    Dim clientSecret As String = "SSP5q4xPndDRru0ut4FelVTg5BrUXRu7"
    Dim dateFrom As String = Date.Now.AddDays(-2).Date.ToString("yyyy-MM-dd") + "T00:00:00"
    Dim dateTo As String = Date.Now.AddDays(1).Date.ToString("yyyy-MM-dd") + "T00:00:00"
    Dim urlGetOrdenes As String = apiMLBase + "orders/search?seller=" + userId.ToString + "&order.status=paid&order.date_created.from=" + dateFrom + ".000-00:00&order.date_created.to=" + dateTo + ".000-00:00"
    Dim httpClient As New HttpClient()

    'Variables para la consulta de ordenes
    Dim ordenes As List(Of Producto)
    Dim respuestaOrdenes As HttpResponseMessage
    Dim bodyOrdenes As String
    Dim tokenML As String
    Dim jsonDoc As JsonDocument
    Dim root As JsonElement

    'Variables para la consulta de clientes
    Dim respuestaBuy As HttpResponseMessage
    Dim ordenId As Long
    Dim urlGetBuy As String
    Dim bodyBuy As String
    Dim jsonClient As JsonDocument
    Dim rootC As JsonElement
    Dim buy As ClienteMl

    ' Peticion para traer datos del envio a partir de una orden
    Dim urlGetshipment As String
    Dim respuestaShip As HttpResponseMessage
    Dim bodyShip As String
    Dim jsonShip As JsonDocument
    Dim rootS As JsonElement

    ' Items
    Dim orderItems As List(Of OrderItem)

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Obtener sucursales
        dtSucs = ConexionBBDD.ConexionSQL.EjecutarSP("sp_obtener_sucursales")

        'cmbSucursales.DataSource = dtSucs
        'cmbSucursales.DisplayMember = "Sucursal"

    End Sub

    Function GetToken() As String

        Dim dtToken As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP_SAP("SP_OBTENER_TOKEN", 1)
        Dim token As String = dtToken.Rows(0).Item(0)
        Return token

    End Function


    Private Async Function ConsultasMLAsync() As Task
        ' Consulto órdenes
        tokenML = GetToken()
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenML)

        respuestaOrdenes = Await httpClient.GetAsync(requestUri:=urlGetOrdenes)
        bodyOrdenes = Await respuestaOrdenes.Content.ReadAsStringAsync()
        jsonDoc = JsonDocument.Parse(bodyOrdenes)
        root = jsonDoc.RootElement

        ordenes = JsonConvert.DeserializeObject(Of List(Of Producto))(root.GetProperty("results").ToString())

        httpClient.DefaultRequestHeaders.Add("x-version", "2")

        ' Iteración de órdenes
        For Each ord In ordenes
            ordenId = ord.Id
            urlGetBuy = apiMLBase & "orders/" & ordenId & "/billing_info"

            ' Petición para traer detalles del cliente a partir de una orden
            respuestaBuy = Await httpClient.GetAsync(urlGetBuy)
            bodyBuy = Await respuestaBuy.Content.ReadAsStringAsync()
            jsonClient = JsonDocument.Parse(bodyBuy)
            rootC = jsonClient.RootElement
            buy = JsonConvert.DeserializeObject(Of ClienteMl)(rootC.ToString())

            ' Petición para traer datos del envío a partir de una orden
            urlGetshipment = apiMLBase & "orders/" & ordenId & "/shipments"
            respuestaShip = Await httpClient.GetAsync(urlGetshipment)
            bodyShip = Await respuestaShip.Content.ReadAsStringAsync()
            jsonShip = JsonDocument.Parse(bodyShip)
            rootS = jsonShip.RootElement

            ' Insertar en cabecera
            If insertarCabecera(ordenId, buy, ord, rootS) = 1 Then
                ' Iteración de artículos dentro de la orden
                orderItems = ord.OrderItems
                Dim headerDate As DateTime = DateTime.Parse(rootS.GetProperty("date_created").ToString())

                For Each item In orderItems
                    InsertarDetalle(ordenId, item, headerDate)
                Next
            End If
        Next
    End Function


    Private Function insertarCabecera(ByVal ordenId As Long, ByVal buy As ClienteMl, ByVal ord As Producto, ByVal rootS As JsonElement)

        Dim CompanyIdNumer As String
        Dim homePhoneNumber As String = ""

        Dim firstName As String = buy.Buyer.BillingInfo.Name
        Dim lastName As String = buy.Buyer.BillingInfo.LastName
        Dim iscompany As Boolean = 0
        Dim dniOrCuit As String = buy.Buyer.BillingInfo.Identification.Number
        Dim addressLine As String = buy.Buyer.BillingInfo.Address.StreetName
        Dim city As String = buy.Buyer.BillingInfo.Address.CityName
        Dim zipCode As String = buy.Buyer.BillingInfo.Address.ZipCode
        Dim fiscalId As String = dniOrCuit
        Dim receiverAddressCity As String = rootS.GetProperty("receiver_address").GetProperty("state").GetProperty("name").ToString
        Dim receiverAddressFirstName As String = rootS.GetProperty("receiver_address").GetProperty("receiver_name").ToString
        Dim receiverAddressLastName As String = rootS.GetProperty("receiver_address").GetProperty("receiver_name").ToString
        Dim receiverAddressLine As String = rootS.GetProperty("receiver_address").GetProperty("address_line").ToString
        Dim receiverAddressZipCode As String = rootS.GetProperty("receiver_address").GetProperty("zip_code").ToString
        Dim headerComment As String = ord.Shipping.Id
        Dim headerCustomerId As String = dniOrCuit
        Dim headerCurrencyId As String = "ARG"
        Dim headerDate As DateTime = rootS.GetProperty("date_created").ToString
        Dim headerInternalReference As String = ordenId
        Dim headerBillingStatus As String = rootS.GetProperty("status").ToString
        Dim headerDeliveryType As String = rootS.GetProperty("shipping_option").GetProperty("delivery_type").ToString
        Dim headerFollowUpStatus As String = "WaitingCommodity"
        Dim headerGiftMessageType As String = "None"
        Dim headerPaymentStatus As String = "Totally"
        Dim headerReturnStatus As String = rootS.GetProperty("return_details").ToString
        Dim headerShippingStatus As String = "Pending"
        Dim headerTransporter As String = "MELI VALLEJO"
        Dim headerStoreId As String = "000102"
        Dim headerWareHouseId As String = "000102"
        Dim headerSalesPersonId As String = "MELI"
        Dim headerType As String = "CustomerOrder"
        Dim amount As String = "0"
        Dim methodId As String = "ECO"
        Dim paymentId As Integer = 20
        Dim dueDate As DateTime = rootS.GetProperty("date_created").ToString
        Dim isReceivedPayment As Boolean = 0
        Dim currencyId As String = "ARG"

        CompanyIdNumer = dniOrCuit
        If buy.Buyer.BillingInfo.Identification.Type = "CUIT" Then
            lastName = firstName + " " + buy.Buyer.BillingInfo.LastName
            firstName = ""
            iscompany = 1
        End If

        ' insercion de datos de cabecera
        Try
            Dim dtMLCabecera As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_INSERTAR_ORDENES_MERCADOLIBRE_CABECERA",
            "VALLEJO",
            dniOrCuit,
            firstName,
            iscompany,
            lastName,
            addressLine,
            city,
            zipCode,
            homePhoneNumber,
            fiscalId,
            CompanyIdNumer,
            receiverAddressCity,
            receiverAddressFirstName,
            receiverAddressLastName,
            receiverAddressLine,
            receiverAddressZipCode,
            headerComment,
            headerCustomerId,
            headerCurrencyId,
            headerDate,
            headerInternalReference,
            headerBillingStatus,
            headerDeliveryType,
            headerFollowUpStatus,
            headerGiftMessageType,
            headerPaymentStatus,
            headerReturnStatus,
            headerShippingStatus,
            headerTransporter,
            headerStoreId,
            headerWareHouseId,
            headerSalesPersonId,
            headerType,
            amount,
            methodId,
            paymentId,
            dueDate,
            isReceivedPayment,
            currencyId
            )

            Return dtMLCabecera.Rows(0).Item(0)

        Catch ex As Exception

        End Try

    End Function



    Sub InsertarDetalle(ByVal ordenId As Long, ByVal item As OrderItem, ByVal headerDate As Date)
        ' Datos del ítem
        Dim reference As String = item.Item.SellerCustomField
        Dim storeId As String = "000102"
        Dim label As String = item.Item.Title.ToString
        Dim deliveryDate As String = headerDate.ToString("dd-MM-yyyy")
        Dim quantity As Integer = item.Quantity
        Dim netUnitPrice As Double = item.UnitPrice
        Dim warehouseId As New List(Of String) From {"000199", "000198"}

        ' Obtener el almacén correspondiente utilizando la función cegid
        Dim warehouse As String = Cegid(item, storeId, warehouseId)

        ' Insertar detalle solo si se obtiene correctamente el almacén
        If warehouse <> "error" Then
            Dim dtMLDetalle As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_INSERTAR_ORDENES_MERCADOLIBRE_DETALLE",
            ordenId,
            reference,
            storeId,
            label,
            deliveryDate,
            quantity,
            netUnitPrice,
            warehouse
        )
        End If
    End Sub



    Function Cegid(ByVal item As OrderItem, ByVal storeId As String, ByVal warehouseId As List(Of String)) As String
        Dim itemIdentifier As New WSProductos.ItemIdentifier
        Dim retailContext As New WSProductos.RetailContext

        ' Configuración del binding y dirección del endpoint
        Dim binding As New BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName

        Dim endpoint As New EndpointAddress("http://cegid.sportotal.com.ar/Y2_VAL/ItemInventoryWcfService.svc")

        ' Crear cliente del servicio
        Dim clientCegid As New ItemInventoryWcfServiceClient(binding, endpoint)

        ' Establecer las credenciales del cliente
        clientCegid.ClientCredentials.UserName.UserName = "VAPRODC\MATIAS"
        clientCegid.ClientCredentials.UserName.Password = "MATIAS2020"

        retailContext.DatabaseId = "VAPRODC"
        itemIdentifier.Reference = item.Item.SellerCustomField

        Try
            For Each wh In warehouseId
                Dim resp As AvailableQtyReturn = clientCegid.GetAvailableQty(item.Item.SellerCustomField, itemIdentifier, storeId, wh, retailContext)
                Dim cantidad As Decimal = resp.AvailableQty

                If cantidad >= item.Quantity Then
                    Return wh ' Devuelve el almacén si hay suficiente cantidad
                End If
            Next

            ' Si no hay suficiente cantidad en ningún almacén, devuelve el primero de la lista
            Return warehouseId.First

        Catch ex As Exception
            Return "error"
        End Try
    End Function


    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Await ConsultasMLAsync()

        Await procesarOrdenes()

    End Sub

    Sub MostrarDatos()

        Dim dt As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_DETALLES")
        dgvDetalles.DataSource = dt

    End Sub



    Function ConsultaClienteCegid(ByVal cliente As String) As Boolean
        Dim retailContext As New WSClientes.RetailContext
        Dim searchData As New CustomerSearchDataType

        ' Configuración del binding y dirección del endpoint
        Dim binding As New BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName

        Dim endpoint As New EndpointAddress("http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc")

        ' Crear cliente del servicio
        Dim clientCegid As New CustomerWcfServiceClient(binding, endpoint)

        ' Establecer las credenciales del cliente
        clientCegid.ClientCredentials.UserName.UserName = "VATEST\MATIAS"
        clientCegid.ClientCredentials.UserName.Password = "MATIAS2020"

        retailContext.DatabaseId = "VATEST"

        ' Establecer parámetros de búsqueda
        searchData.FiscalId = cliente

        Try
            ' Realizar la búsqueda del cliente
            Dim resp = clientCegid.SearchCustomerIds(searchData, retailContext)

            ' Verificar si se encontró algún cliente
            Return resp.Count <> 0

        Catch ex As Exception
            ' Manejar cualquier excepción que pueda ocurrir durante la consulta
            Return False
        End Try
    End Function


    Private Async Function ProcesarOrdenes() As Task
        ' Obtener las órdenes de la base de datos
        Dim dtCab As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_ORDENES_MELI_CABECERA")

        If dtCab IsNot Nothing AndAlso dtCab.Rows.Count > 0 Then
            For Each orden As DataRow In dtCab.Rows
                Dim dni As String = orden.Field(Of String)("CustomerId")

                ' Verificar si el cliente existe en Cegid
                If Not ConsultaClienteCegid(dni) Then
                    ' Si el cliente no existe, crearlo
                    CrearCliente(orden)
                End If

                ' Crear la orden
                Await CrearOrden(orden)

                ' Obtener los ítems de la orden si es necesario
                ' (aquí se puede implementar según la lógica específica)

            Next
        End If

    End Function


    Private Async Function CrearOrden(ByVal orden As DataRow) As Task
        ' Declaración de variables
        Dim retailContext As New WSorden.RetailContext
        Dim createRequest As New Create_Request
        Dim binding = New BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
        Dim endpoint As New EndpointAddress("http://cegid.sportotal.com.ar/Y2_VAL/SaleDocumentService.svc?singleWsdl")
        Dim clientCegid As SaleDocumentServiceClient

        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName
        clientCegid = New SaleDocumentServiceClient(binding, endpoint)
        clientCegid.ClientCredentials.UserName.UserName = "VATEST\MATIAS"
        clientCegid.ClientCredentials.UserName.Password = "MATIAS2020"
        retailContext.DatabaseId = "VATEST"

        ' Obtención de los items de la orden desde la base de datos
        Dim orderItems As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_ORDENES_MELI_DETALLE", orden.Field(Of String)("Header_InternalReference"))

        ' Creación de objetos para la orden
        Dim deliveryAddress As New WSorden.Address()
        Dim createHeader As New WSorden.Create_Header()
        Dim omniChannel As New WSorden.OmniChannel()
        Dim payments As WSorden.Create_Payment() = {New WSorden.Create_Payment()}
        Dim createLineList As New List(Of WSorden.Create_Line)()

        ' Configuración de la dirección de entrega
        deliveryAddress.City = orden.Field(Of String)("DeliveryAddress_City")
        deliveryAddress.CountryId = "ARS"
        deliveryAddress.CountryIdType = WSorden.CountryIdType.Internal
        deliveryAddress.FirstName = orden.Field(Of String)("DeliveryAddress_FirstName")
        deliveryAddress.LastName = orden.Field(Of String)("DeliveryAddress_LastName")
        deliveryAddress.Line1 = orden.Field(Of String)("DeliveryAddress_Line1")
        deliveryAddress.ZipCode = orden.Field(Of String)("DeliveryAddress_ZipCode")

        ' Configuración del encabezado de la orden
        createHeader.Active = True
        createHeader.Comment = orden.Field(Of String)("Header_Comment")
        createHeader.CustomerId = orden.Field(Of String)("Header_CustomerId")
        createHeader.CurrencyId = orden.Field(Of String)("Header_CurrencyId")
        createHeader.Date = Date.Parse(orden.Item("Header_Date").ToString).ToString("dd-MM-yyyy")
        createHeader.InternalReference = orden.Field(Of String)("Header_InternalReference")
        createHeader.Origin = WSorden.DocumentOrigin.ECommerce

        ' Configuración del canal omni
        omniChannel.BillingStatus = WSorden.BillingStatus.Pending
        omniChannel.DeliveryType = WSorden.DeliveryType.ShipByCentral
        omniChannel.FollowUpStatus = If(orden.Field(Of String)("Header_InternalReference").Contains("SPLIT"), WSorden.FollowUpStatus.Validated, WSorden.FollowUpStatus.WaitingCommodity)
        omniChannel.GiftMessageType = WSorden.GiftMessageType.None
        omniChannel.PaymentStatus = WSorden.PaymentStatus.Totally
        omniChannel.ReturnStatus = WSorden.OrderReturnStatus.NotReturned
        omniChannel.ShippingStatus = WSorden.ShippingStatus.Pending
        omniChannel.Transporter = "MELI VALLEJO"

        createHeader.OmniChannel = omniChannel

        ' Configuración adicional según la condición SPLIT
        If orden.Field(Of String)("Header_InternalReference").Contains("SPLIT") Then
            createHeader.StoreId = orden.Field(Of String)("Header_StoreId")
            createHeader.WarehouseId = "000198"
            createHeader.OmniChannel.FollowUpStatus = WSorden.FollowUpStatus.Validated
        Else
            createHeader.StoreId = orden.Field(Of String)("Header_StoreId")
            createHeader.WarehouseId = orden.Field(Of String)("Header_WarehouseId")
        End If

        createHeader.SalesPersonId = orden.Field(Of String)("Header_SalesPersonId")
        createHeader.Type = WSorden.SaleDocumentType.CustomerOrder

        ' Configuración de los pagos
        payments(0).Amount = 0
        payments(0).MethodId = "ECO"
        payments(0).Id = 20
        payments(0).DueDate = Date.Parse(orden.Item("Header_Date").ToString).ToString("dd-MM-yyyy")
        payments(0).IsReceivedPayment = False
        payments(0).CurrencyId = "ARG"

        ' Creación de líneas de la orden
        For Each item As DataRow In orderItems.Rows
            Dim newCreateLine As New WSorden.Create_Line()
            Dim itemIdentifier As New WSorden.ItemIdentifier()
            Dim omniChannelLine As New WSorden.OmniChannelLine()

            itemIdentifier.Reference = item.Field(Of String)("Reference")

            newCreateLine.Label = item.Field(Of String)("Label")
            newCreateLine.Origin = WSorden.DocumentOrigin.ECommerce
            newCreateLine.DeliveryDate = Date.Parse(item.Item("DeliveryDate").ToString).ToString("dd-MM-yyyy")
            newCreateLine.Quantity = Integer.Parse(item.Item("Quantity"))
            newCreateLine.NetUnitPrice = Decimal.Parse(item.Item("NetUnitPrice"))

            If item.Field(Of String)("WarehouseId") <> "000198" Then
                omniChannelLine.WarehouseId = item.Field(Of String)("WarehouseId")
            End If

            newCreateLine.SalesPersonId = item.Field(Of String)("SalesPersonId")

            newCreateLine.ItemIdentifier = itemIdentifier
            newCreateLine.OmniChannel = omniChannelLine

            createLineList.Add(newCreateLine)
        Next

        createRequest.DeliveryAddress = deliveryAddress
        createRequest.Header = createHeader
        createRequest.Lines = createLineList.ToArray()
        createRequest.Payments = payments

        Try
            ' Llamar al servicio para crear la orden
            Dim resp = Await clientCegid.CreateAsync(createRequest, retailContext)

            ' Actualizar la orden en la base de datos después de la creación exitosa
            ConexionBBDD.ConexionSQL.EjecutarSP("SP_UPDATE_ORDENES_MELI_CABECERA", orden.Field(Of Integer)("id"))

        Catch ex As Exception
            ' Manejar cualquier excepción que pueda ocurrir durante la creación de la orden
            Console.WriteLine($"Error al crear la orden: {ex.Message}")

        End Try
    End Function


    Async Sub CrearCliente(ByVal orden As Object)
        ' Declaración de variables
        Dim retailContext As New WSClientes.RetailContext
        Dim searchData As New CustomerSearchDataType
        Dim customerInsert As New CustomerInsertData
        Dim addresData As New AddressDataType
        Dim phoneData As New PhoneDataType

        ' Configuración del cliente del servicio
        Dim binding As New BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
        Dim endpoint As New EndpointAddress("http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc?singleWsdl")
        Dim clientCegid As CustomerWcfServiceClient

        ' Configuración de las credenciales
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName

        clientCegid = New CustomerWcfServiceClient(binding, endpoint)
        clientCegid.ClientCredentials.UserName.UserName = "VATEST\MATIAS"
        clientCegid.ClientCredentials.UserName.Password = "MATIAS2020"
        retailContext.DatabaseId = "VATEST"

        Try
            ' Asignación de datos del cliente
            customerInsert.CustomerId = orden.Item("CustomerId").ToString
            customerInsert.FirstName = If(orden.Item("FirstName").ToString = "", "", orden.Item("FirstName").ToString)
            customerInsert.IsCompany = If(orden.Item("IsCompany").ToString = "True", True, False)
            customerInsert.LastName = orden.Item("LastName").ToString

            addresData.AddressLine1 = orden.Item("AddressLine1").ToString
            addresData.City = orden.Item("City").ToString
            addresData.CountryId = "ARS"
            addresData.CountryIdType = WSClientes.CountryIdType.Internal
            addresData.ZipCode = orden.Item("ZipCode").ToString
            customerInsert.AddressData = addresData

            phoneData.HomePhoneNumber = orden.Item("HomePhoneNumber").ToString
            customerInsert.PhoneData = phoneData
            customerInsert.UsualStoreId = "000102"
            customerInsert.FiscalId = orden.Item("FiscalId").ToString

            If customerInsert.IsCompany Then
                customerInsert.LastName = orden.Item("FirstName").ToString + orden.Item("LastName").ToString
                customerInsert.CompanyIdNumber = orden.Item("CompanyIdNumber").ToString
            End If

            customerInsert.VATSystem = "TAX"

            ' Llamada al servicio para agregar un nuevo cliente
            clientCegid.AddNewCustomer(customerInsert, retailContext)

        Catch ex As Exception
            ' Manejo de excepciones
            Console.WriteLine($"Error al crear cliente: {ex.Message}")
        End Try
    End Sub


End Class



