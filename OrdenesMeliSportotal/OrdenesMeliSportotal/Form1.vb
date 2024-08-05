Imports System.Globalization
Imports System.Net.Http
Imports System.ServiceModel
Imports System.Text.Json
Imports Newtonsoft.Json
Imports OrdenesMeliSportotal.WSClientes
Imports OrdenesMeliSportotal.WSOrden

Public Class Form1
    'Datos ML
    Dim ReadOnly apiMLBase As String = "https://api.mercadolibre.com/"
    Dim ReadOnly userId As Integer = 209369664
    Dim clientId As String = "5038796649356510"
    Dim clientSecret As String = "SSP5q4xPndDRru0ut4FelVTg5BrUXRu7"
    Dim ReadOnly dateFrom As String = Date.Now.AddDays(- 1).Date.ToString("yyyy-MM-dd") + "T00:00:00"
    Dim ReadOnly dateTo As String = Date.Now.Date.ToString("yyyy-MM-dd") + "T00:00:00"

    ReadOnly _
        urlGetOrdenes As String = apiMLBase + "orders/search?seller=" + userId.ToString +
                                  "&order.status=paid&order.date_created.from=" + dateFrom +
                                  ".000-00:00&order.date_created.to=" + dateTo + ".000-00:00"

    Dim ReadOnly httpClient As New HttpClient()

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
    Dim buy As ClienteML

    ' Peticion para traer datos del envio a partir de una orden
    Dim urlGetshipment As String
    Dim respuestaShip As HttpResponseMessage
    Dim bodyShip As String
    Dim jsonShip As JsonDocument
    Dim rootS As JsonElement

    ' Items
    Dim orderItems As List(Of OrderItem)

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await ConsultasMLAsync()
        'Await ProcesarOrdenes()
        Dispose()
    End Sub

    Function GetToken() As String

        Dim dtToken As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP_SAP("SP_OBTENER_TOKEN", 2)
        Dim token As String = dtToken.Rows(0).Item(0)
        Return token
    End Function

    Private Async Function ConsultasMLAsync() As Task
        Dim total As Integer
        Dim offset As Integer
        Dim limit As Integer

        ' Consulto órdenes
        tokenML = GetToken()
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenML)

        respuestaOrdenes = Await httpClient.GetAsync(requestUri:=urlGetOrdenes + "&offset=0")
        bodyOrdenes = Await respuestaOrdenes.Content.ReadAsStringAsync()
        jsonDoc = JsonDocument.Parse(bodyOrdenes)
        root = jsonDoc.RootElement

        If Not Integer.TryParse(root.GetProperty("paging").GetProperty("total").ToString, total) Or
           Not Integer.TryParse(root.GetProperty("paging").GetProperty("offset").ToString, offset) Or
           Not Integer.TryParse(root.GetProperty("paging").GetProperty("limit").ToString, limit) Then
            Exit Function
        End If

        For pagina As Integer = offset To total Step limit
            respuestaOrdenes = Await httpClient.GetAsync(requestUri:=urlGetOrdenes + "&offset=" + pagina.ToString())
            bodyOrdenes = Await respuestaOrdenes.Content.ReadAsStringAsync()
            jsonDoc = JsonDocument.Parse(bodyOrdenes)
            root = jsonDoc.RootElement
            ordenes = JsonConvert.DeserializeObject(Of List(Of Producto))(root.GetProperty("results").ToString())

            ' Header necesario para billing info
            httpClient.DefaultRequestHeaders.Add("x-version", "2")

            ' Iteración de órdenes
            For Each ord In ordenes
                Dim packIdAux As String = ""
                Try
                    If ord.PackId IsNot Nothing Then
                        packIdAux = ord.PackId
                    End If

                    ordenId = ord.Id ' ------------ seguir aqui

                    ' Petición para traer detalles del cliente a partir de una orden
                    urlGetBuy = apiMLBase & "orders/" & ordenId & "/billing_info"
                    respuestaBuy = Await httpClient.GetAsync(urlGetBuy)
                    bodyBuy = Await respuestaBuy.Content.ReadAsStringAsync()
                    jsonClient = JsonDocument.Parse(bodyBuy)
                    rootC = jsonClient.RootElement
                    buy = JsonConvert.DeserializeObject(Of ClienteML)(rootC.ToString())

                    ' Petición para traer datos del envío a partir de una orden
                    urlGetshipment = apiMLBase & "orders/" & ordenId & "/shipments"
                    respuestaShip = Await httpClient.GetAsync(urlGetshipment)
                    bodyShip = Await respuestaShip.Content.ReadAsStringAsync()
                    jsonShip = JsonDocument.Parse(bodyShip)
                    rootS = jsonShip.RootElement
                Catch ex As Exception

                End Try
                'Insertar en cabecera
                If InsertarCabecera(ordenId, buy, ord, rootS, packIdAux) = 1 Then
                    ' Iteración de artículos dentro de la orden
                    orderItems = ord.OrderItems
                    Dim headerDate As DateTime = DateTime.Parse(rootS.GetProperty("date_created").ToString())

                    For Each item In orderItems
                        If packIdAux <> "" Then
                            ordenId = packIdAux
                        End If
                        InsertarDetalle(ordenId, item, headerDate)
                    Next
                End If
            Next

        Next

    End Function

    Private Function InsertarCabecera(ordenId As Long, buy As ClienteML, ord As Producto, rootS As JsonElement, packId As String)

        Dim CompanyIdNumer As String
        Dim homePhoneNumber = ""

        Dim firstName As String = buy.Buyer.BillingInfo.Name
        Dim lastName As String = buy.Buyer.BillingInfo.LastName
        Dim iscompany As Boolean = 0
        Dim dniOrCuit As String = buy.Buyer.BillingInfo.Identification.Number
        Dim addressLine As String = buy.Buyer.BillingInfo.Address.StreetName
        Dim city As String = buy.Buyer.BillingInfo.Address.CityName
        Dim zipCode As String = buy.Buyer.BillingInfo.Address.ZipCode
        Dim fiscalId As String = dniOrCuit
        Dim receiverAddressCity As String =
                rootS.GetProperty("receiver_address").GetProperty("state").GetProperty("name").ToString
        Dim receiverAddressFirstName As String =
                rootS.GetProperty("receiver_address").GetProperty("receiver_name").ToString
        Dim receiverAddressLastName As String =
                rootS.GetProperty("receiver_address").GetProperty("receiver_name").ToString
        Dim receiverAddressLine As String = rootS.GetProperty("receiver_address").GetProperty("address_line").ToString
        Dim receiverAddressZipCode As String = rootS.GetProperty("receiver_address").GetProperty("zip_code").ToString
        Dim headerComment As String = ord.Shipping.Id
        Dim headerCustomerId As String = dniOrCuit
        Dim headerCurrencyId = "ARG"


        Dim headerDate2 As DateTime = DateTime.Parse(rootS.GetProperty("date_created").ToString())
        Dim formattedDate As String = headerDate2.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)

        Dim headerDate As String = formattedDate
        'Dim headerInternalReference As String = ordenId 
        Dim headerInternalReference As String = If(packId <> "", packId, ordenId)
        Dim headerBillingStatus As String = rootS.GetProperty("status").ToString
        Dim headerDeliveryType As String = rootS.GetProperty("shipping_option").GetProperty("delivery_type").ToString
        Dim headerFollowUpStatus = "WaitingCommodity"
        Dim headerGiftMessageType = "None"
        Dim headerPaymentStatus = "Totally"
        Dim headerReturnStatus As String = rootS.GetProperty("return_details").ToString
        Dim headerShippingStatus = "Pending"
        Dim headerTransporter = "MELI SPT"
        Dim headerStoreId = "000111"
        Dim headerWareHouseId = "000111"
        Dim headerSalesPersonId = "MELI"
        Dim headerType = "CustomerOrder"
        Dim amount = "0"
        Dim methodId = "ECO"
        Dim paymentId = 20
        Dim dueDate As DateTime = rootS.GetProperty("date_created").ToString
        Dim isReceivedPayment As Boolean = 0
        Dim currencyId = "ARG"

        CompanyIdNumer = dniOrCuit
        If buy.Buyer.BillingInfo.Identification.Type = "CUIT" Then
            lastName = firstName + " " + buy.Buyer.BillingInfo.LastName
            firstName = ""
            iscompany = 1
        End If

        ' insercion de datos de cabecera
        Try
            Dim dtMLCabecera As DataTable =
                    ConexionBBDD.ConexionSQL.EjecutarSP("SP_INSERTAR_ORDENES_MERCADOLIBRE_CABECERA",
                                                        "SPORTOTAL",
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

            Dim resp = dtMLCabecera.Rows(0).Item(0)

            If (resp = 0 And packId <> "") Then
                Return 1
            End If

            Return resp


        Catch ex As Exception
            Return 0
        End Try
    End Function


    Sub InsertarDetalle(ordenId As Long, item As OrderItem, headerDate As Date)
        ' Datos del ítem
        try

     
        Dim reference As String = item.Item.SellerCustomField
        Dim storeId = "000111"
        Dim label As String = item.Item.Title.ToString
        Dim deliveryDate As Date = headerDate.ToString("yyyy-MM-dd")
        Dim quantity As Integer = item.Quantity
        Dim netUnitPrice As Double = item.UnitPrice
        Dim warehouse = "000198"

        ' Insertar detalle solo si se obtiene correctamente el almacén
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
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Function ConsultaClienteCegid(cliente As String) As Boolean
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
            MsgBox("error en ConsultaClienteCegid" + ex.Message)
            Return False
        End Try
    End Function

    Private Async Function ProcesarOrdenes() As Task
        Try

        
        ' Obtener las órdenes de la base de datos
        Dim dtCab As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_ORDENES_MELI_CABECERA", "SPORTOTAL")

        If dtCab IsNot Nothing AndAlso dtCab.Rows.Count > 0 Then
            For Each orden As DataRow In dtCab.Rows
                Dim dni = orden.Field (Of String)("CustomerId")

                ' Verificar si el cliente existe en Cegid
                If Not ConsultaClienteCegid(dni) Then
                    ' Si el cliente no existe, crearlo
                    CrearCliente(orden)
                End If

                ' Crear la orden
                Await CrearOrden(orden)

            Next
        End If
        Catch ex As Exception
            MsgBox("error en ProcesarOrdenes" + ex.Message)
        End Try
    End Function


    Private Async Function CrearOrden(orden As DataRow) As Task
        Try

       
        ' Declaración de variables
        Dim retailContext As New WSOrden.RetailContext
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
        Dim orderItems As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_ORDENES_MELI_DETALLE",
                                                                          orden.Field (Of String)(
                                                                              "Header_InternalReference"))

        ' Creación de objetos para la orden
        Dim deliveryAddress As New WSOrden.Address()
        Dim createHeader As New Create_Header()
        Dim omniChannel As New OmniChannel()
        Dim payments As Create_Payment() = {New Create_Payment()}
        Dim createLineList As New List(Of Create_Line)()

        ' Configuración de la dirección de entrega
        deliveryAddress.City = orden.Field (Of String)("DeliveryAddress_City")
        deliveryAddress.CountryId = "ARS"
        deliveryAddress.CountryIdType = WSOrden.CountryIdType.Internal
        deliveryAddress.FirstName = orden.Field (Of String)("DeliveryAddress_FirstName")
        deliveryAddress.LastName = orden.Field (Of String)("DeliveryAddress_LastName")
        deliveryAddress.Line1 = orden.Field (Of String)("DeliveryAddress_Line1")
        deliveryAddress.ZipCode = orden.Field (Of String)("DeliveryAddress_ZipCode")

        ' Configuración del encabezado de la orden
        createHeader.Active = True
        createHeader.Comment = orden.Field (Of String)("Header_Comment")
        createHeader.CustomerId = orden.Field (Of String)("Header_CustomerId")
            createHeader.CurrencyId = orden.Field(Of String)("Header_CurrencyId")

            Dim headerDate As DateTime = DateTime.Parse(orden.Item("Header_Date").ToString())
            Dim formattedHeaderDate As String = headerDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)

            createHeader.Date = formattedHeaderDate
            createHeader.InternalReference = orden.Field (Of String)("Header_InternalReference")
        createHeader.Origin = DocumentOrigin.ECommerce

        ' Configuración del canal omni
        omniChannel.BillingStatus = BillingStatus.Pending
        omniChannel.DeliveryType = DeliveryType.ShipByCentral
        omniChannel.FollowUpStatus =
            If _
                (orden.Field (Of String)("Header_InternalReference").Contains("SPLIT"), FollowUpStatus.Validated,
                 FollowUpStatus.WaitingCommodity)
        omniChannel.GiftMessageType = GiftMessageType.None
        omniChannel.PaymentStatus = PaymentStatus.Totally
        omniChannel.ReturnStatus = OrderReturnStatus.NotReturned
        omniChannel.ShippingStatus = ShippingStatus.Pending
        omniChannel.Transporter = "MELI TESI"

        createHeader.OmniChannel = omniChannel
        createHeader.StoreId = orden.Field (Of String)("Header_StoreId")
        createHeader.WarehouseId = orden.Field (Of String)("Header_WarehouseId")
        createHeader.SalesPersonId = orden.Field (Of String)("Header_SalesPersonId")
        createHeader.Type = SaleDocumentType.CustomerOrder

        ' Configuración de los pagos
        payments(0).Amount = 0
        payments(0).MethodId = "ECO"
        payments(0).Id = 20
        payments(0).DueDate = Date.Parse(orden.Item("Header_Date").ToString).ToString("dd-MM-yyyy")
        payments(0).IsReceivedPayment = False
        payments(0).CurrencyId = "ARG"

        ' Creación de líneas de la orden
        For Each item As DataRow In orderItems.Rows
            Dim newCreateLine As New Create_Line()
            Dim itemIdentifier As New ItemIdentifier()
            Dim omniChannelLine As New OmniChannelLine()

            itemIdentifier.Reference = item.Field (Of String)("Reference")

            newCreateLine.Label = item.Field (Of String)("Label")
                newCreateLine.Origin = DocumentOrigin.ECommerce
                Dim deliveryDate As DateTime = DateTime.Parse(item.Item("DeliveryDate").ToString())
                Dim formattedDeliveryDate As String = deliveryDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                newCreateLine.DeliveryDate = formattedDeliveryDate
                newCreateLine.Quantity = Integer.Parse(item.Item("Quantity"))
            newCreateLine.NetUnitPrice = Decimal.Parse(item.Item("NetUnitPrice"))
            newCreateLine.SalesPersonId = item.Field (Of String)("SalesPersonId")
            newCreateLine.ItemIdentifier = itemIdentifier
            omniChannelLine.WarehouseId = item.Field (Of String)("WarehouseId") '

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
            ConexionBBDD.ConexionSQL.EjecutarSP("SP_UPDATE_ORDENES_MELI_CABECERA", orden.Field (Of Integer)("id"))

        Catch ex As Exception
            ' Manejar cualquier excepción que pueda ocurrir durante la creación de la orden
            MsgBox($"Error al crear la orden: {ex.Message}")

        End Try
        Catch ex As Exception
            MsgBox("error en crearOrdenCegid "+ ex.Message)
        End Try
    End Function

    Async Sub CrearCliente(orden As Object)
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
            customerInsert.UsualStoreId = "000111"
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
            MsgBox($"Error al crear cliente: {ex.Message}")
        End Try
    End Sub
End Class
