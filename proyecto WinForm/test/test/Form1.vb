Imports System.Net.Http
Imports System.ServiceModel
Imports System.Text.Json
Imports Newtonsoft.Json
Imports test.ClienteMl
Imports test.Producto
Imports test.WSProductos
Imports test.WSClientes

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

    Function getToken() As String

        Dim dtToken As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP_SAP("SP_OBTENER_TOKEN", 1)
        Dim token As String = dtToken.Rows(0).Item(0)
        Return token

    End Function

    Private Async Sub consultasMLAsync()

        'Consulto ordenes
        tokenML = getToken()
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenML)

        respuestaOrdenes = Await httpClient.GetAsync(urlGetOrdenes)
        bodyOrdenes = Await respuestaOrdenes.Content.ReadAsStringAsync
        jsonDoc = JsonDocument.Parse(bodyOrdenes)
        root = jsonDoc.RootElement

        ordenes = JsonConvert.DeserializeObject(Of List(Of Producto))(root.GetProperty("results").ToString)

        httpClient.DefaultRequestHeaders.Add("x-version", 2)

        'iteracion de ordenes
        For Each ord In ordenes

            ordenId = ord.Id
            urlGetBuy = apiMLBase & "orders/" & ordenId & "/billing_info"

            ' Peticion para traer detalles del cliente a partir de una orden
            respuestaBuy = Await httpClient.GetAsync(urlGetBuy)
            bodyBuy = Await respuestaBuy.Content.ReadAsStringAsync
            jsonClient = JsonDocument.Parse(bodyBuy)
            rootC = jsonClient.RootElement
            buy = JsonConvert.DeserializeObject(Of ClienteMl)(rootC.ToString)

            ' Peticion para traer datos del envio a partir de una orden
            urlGetshipment = apiMLBase & "orders/" & ordenId & "/shipments"
            respuestaShip = Await httpClient.GetAsync(urlGetshipment)
            bodyShip = Await respuestaShip.Content.ReadAsStringAsync
            jsonShip = JsonDocument.Parse(bodyShip)
            rootS = jsonShip.RootElement

            ' Insertar en cabecera
            insertarCabecera(ordenId, buy, ord, rootS)

            ' Iteracion de articulos dentro de la orden
            orderItems = ord.OrderItems
            Dim headerDate As DateTime = rootS.GetProperty("date_created").ToString

            For Each item In orderItems

                insertarDetalle(ordenId, item, headerDate)

            Next

        Next

        MostrarDatos()

    End Sub

    Sub insertarCabecera(ByVal ordenId As Long, ByVal buy As ClienteMl, ByVal ord As Producto, ByVal rootS As JsonElement)

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

    End Sub

    Sub insertarDetalle(ByVal ordenId As Long, ByVal item As OrderItem, ByVal headerDate As Date)

        Dim reference As String = item.Item.SellerCustomField
        Dim storeId As String = "000102"
        Dim label As String = item.Item.Title.ToString
        Dim deliveryDate As String = headerDate.ToString("dd-MM-yyyy")
        Dim quantity As Integer = item.Quantity
        Dim netUnitPrice As Double = item.UnitPrice
        Dim warehouseId As New List(Of String) From {"000198", "000199", "no encontrado"}

        Dim warehouse As String = cegid(item, storeId, warehouseId)

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

    End Sub

    Function cegid(ByVal item As OrderItem, ByVal storeId As String, ByVal warehouseId As List(Of String)) As String

        Dim itemIdentifier As New ItemIdentifier
        Dim retailContext As New WSProductos.RetailContext

        ' Crear una instancia del cliente del servicio
        Dim binding = New BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)

        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName

        Dim endpoint As New EndpointAddress("http://cegid.sportotal.com.ar/Y2_VAL/ItemInventoryWcfService.svc?singleWsdl")

        Dim clientCegid As New ItemInventoryWcfServiceClient(binding, endpoint)

        Dim cantidad As Decimal = 0
        Dim warehouse As String = ""

        ' Establecer las credenciales
        clientCegid.ClientCredentials.UserName.UserName = "VAPRODC\MATIAS"
        clientCegid.ClientCredentials.UserName.Password = "MATIAS2020"

        retailContext.DatabaseId = "VAPRODC"

        '  itemIdentifier.Id = item.Item.SellerCustomField
        itemIdentifier.Reference = item.Item.SellerCustomField


        Try
            For Each wh In warehouseId
                If wh <> warehouseId.Last Then
                    Dim resp As AvailableQtyReturn = clientCegid.GetAvailableQty(item.Item.SellerCustomField, itemIdentifier, storeId.ToString, wh.ToString, retailContext)
                    cantidad = resp.AvailableQty
                End If

                If cantidad >= 1 Or wh = warehouseId.Last Then
                    warehouse = wh
                    Exit For
                End If
            Next

            Return warehouse

        Catch ex As Exception
            Return "error"
        End Try

    End Function

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        crearOrden()
        'consultasMLAsync()
        'Dim d As String = "31366430"
        'Dim d2 As String = "23545062"

        'ConsultaClienteCegid(d)

    End Sub

    Sub MostrarDatos()

        Dim dt As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_DETALLES")
        dgvDetalles.DataSource = dt

    End Sub

    Function ConsultaClienteCegid(ByVal cliente As String) As Boolean

        Dim retailContext As New WSClientes.RetailContext
        Dim searchData As New CustomerSearchDataType
        'Dim respuesta As CustomerQueryData

        ' Crear una instancia del cliente del servicio
        Dim binding = New BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
        Dim endpoint As New EndpointAddress("http://cegid.sportotal.com.ar/Y2_VAL/CustomerWcfService.svc?singleWsdl")
        Dim clientCegid As CustomerWcfServiceClient

        ' Establecer configuraciones
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName

        ' Establecer las credenciales
        clientCegid = New CustomerWcfServiceClient(binding, endpoint)
        clientCegid.ClientCredentials.UserName.UserName = "VAPRODC\MATIAS"
        clientCegid.ClientCredentials.UserName.Password = "MATIAS2020"
        retailContext.DatabaseId = "VAPRODC"

        ' Establecer parametros de busqueda
        'searchData.FiscalId = cliente.Buyer.BillingInfo.Identification.Number.ToString

        searchData.FiscalId = cliente

        Dim resp = clientCegid.SearchCustomerIds(searchData, retailContext)
        Dim r = resp.Count

        Return resp.Count = 0

    End Function

    Function crearOrden()
        Dim dtCab As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_ORDENES_MELI_CABECERA")

        For Each orden In dtCab.Rows
            Dim dni As String = orden.Item("CustomerId").ToString
            Dim ordenId = orden.Item("Header_InternalReference").ToString

            'CREAR ORDEN Y CLIENTE EN VATEST
            'If ConsultaClienteCegid(dni) Then
            '    'creo orden
            '    MsgBox("existeCliente")
            'Else
            '    'creo cliente
            '    MsgBox("No existe")
            'End If

            Dim dtDet As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_ORDENES_MELI_DETALLE", ordenId)

            For Each item In dtDet.Rows
                MsgBox(item("Label"))
            Next


        Next

        'For i = 0 To dtCab.Rows.Count - 1
        '    Dim OrdenId = dtCab.Rows(i).Item("Header_InternalReference")
        '    Dim DNI = dtCab.Rows(i).Item("CustomerId")

        '    'CREAR ORDEN Y CLIENTE EN VATEST

        '    If ConsultaClienteCegid(DNI) Then
        '        'creo orden
        '        MsgBox("existeCliente")
        '    Else
        '        'creo cliente
        '        MsgBox("No existe")
        '    End If

        '    Dim dtDet As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_OBTENER_ORDENES_MELI_DETALLE", OrdenId)

        '    For j = 0 To dtDet.Rows.Count - 1

        '        MsgBox(dtDet.Rows(j).Item("Label"))
        '    Next

        'Next

    End Function
End Class



