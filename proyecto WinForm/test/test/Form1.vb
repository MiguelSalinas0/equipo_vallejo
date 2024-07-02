Imports System.Net.Http
Imports System.ServiceModel
Imports System.Text.Json
Imports Newtonsoft.Json
Imports test.ClienteMl
Imports test.Producto
Imports test.ServiceReference1

Public Class Form1

    'Dim dtSucs As DataTable

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

        'Establecer la conexion a DB 


        'dtSucs = ConexionBBDD.ConexionSQL.EjecutarSP("sp_obtener_sucursales")

        'cmbSucursales.DataSource = dtSucs
        'cmbSucursales.DisplayMember = "Sucursal"


        'Consulta a ML
        'httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token)

        'Dim respuesta As HttpResponseMessage = Await httpClient.GetAsync(urlGetOrdenes)
        'Dim body = Await respuesta.Content.ReadAsStringAsync

        'Dim jsonDoc As JsonDocument = JsonDocument.Parse(body)
        'Dim root As JsonElement = jsonDoc.RootElement


        '' tipeo de las ordenes
        'ordenes = JsonConvert.DeserializeObject(Of List(Of Producto))(root.GetProperty("results").ToString)
        'Dim ordenId As Long
        'Dim urlGetBuy As String
        'Dim respuestaBuy As HttpResponseMessage
        'Dim bodyBuy As String
        'Dim jsonClient As JsonDocument
        'Dim rootC As JsonElement
        'HttpClient.DefaultRequestHeaders.Add("x-version", 2)

        'iteracion de ordenes y obtencion de informacion de cliente
        'For Each ord In ordenes
        '    ordenId = ord.Id
        '    urlGetBuy = apiMLBase & "orders/" & ordenId & "/billing_info"

        '    ' Peticion para traer detalles del cliente a partir de una orden
        '    respuestaBuy = Await httpClient.GetAsync(urlGetBuy)
        '    bodyBuy = Await respuestaBuy.Content.ReadAsStringAsync
        '    jsonClient = JsonDocument.Parse(bodyBuy)
        '    rootC = jsonClient.RootElement
        '    Dim buy As ClienteMl = JsonConvert.DeserializeObject(Of ClienteMl)(rootC.ToString)

        '    ' Peticion para traer datos del envio a partir de una orden
        '    Dim urlGetshipment As String = apiMLBase & "orders/" & ordenId & "/shipments"
        '    Dim respuestaShip = Await httpClient.GetAsync(urlGetshipment)
        '    Dim bodyShip = Await respuestaShip.Content.ReadAsStringAsync
        '    Dim jsonShip = JsonDocument.Parse(bodyShip)
        '    Dim rootS = jsonShip.RootElement


        '    ' Definicion de parametros para SP
        '    Dim firstName As String = buy.Buyer.BillingInfo.Name
        '    Dim lastName As String = buy.Buyer.BillingInfo.LastName
        '    Dim iscompany As Boolean = 0
        '    Dim dniOrCuit As String = buy.Buyer.BillingInfo.Identification.Number
        '    Dim CompanyIdNumer As String
        '    Dim addressLine As String = buy.Buyer.BillingInfo.Address.StreetName
        '    Dim city As String = buy.Buyer.BillingInfo.Address.CityName
        '    Dim zipCode As String = buy.Buyer.BillingInfo.Address.ZipCode
        '    Dim homePhoneNumber As String = ""
        '    Dim fiscalId As String = dniOrCuit
        '    Dim receiverAddressCity As String = rootS.GetProperty("receiver_address").GetProperty("state").GetProperty("name").ToString
        '    Dim receiverAddressFirstName As String = rootS.GetProperty("receiver_address").GetProperty("receiver_name").ToString
        '    Dim receiverAddressLastName As String = rootS.GetProperty("receiver_address").GetProperty("receiver_name").ToString
        '    Dim receiverAddressLine As String = rootS.GetProperty("receiver_address").GetProperty("address_line").ToString
        '    Dim receiverAddressZipCode As String = rootS.GetProperty("receiver_address").GetProperty("zip_code").ToString
        '    Dim headerComment As String = ord.Shipping.Id
        '    Dim headerCustomerId As String = dniOrCuit
        '    Dim headerCurrencyId As String = "ARG"
        '    Dim headerDate As DateTime = rootS.GetProperty("date_created").ToString
        '    Dim headerInternalReference As String = ordenId
        '    Dim headerBillingStatus As String = rootS.GetProperty("status").ToString
        '    Dim headerDeliveryType As String = rootS.GetProperty("shipping_option").GetProperty("delivery_type").ToString
        '    Dim headerFollowUpStatus As String = "WaitingCommodity"
        '    Dim headerGiftMessageType As String = "None"
        '    Dim headerPaymentStatus As String = "Totally"
        '    Dim headerReturnStatus As String = rootS.GetProperty("return_details").ToString
        '    Dim headerShippingStatus As String = "Pending"
        '    Dim headerTransporter As String = "MELI VALLEJO"
        '    Dim headerStoreId As String = "000102"
        '    Dim headerWareHouseId As String = "000102"
        '    Dim headerSalesPersonId As String = "MELI"
        '    Dim headerType As String = "CustomerOrder"
        '    Dim amount As String = "0"
        '    Dim methodId As String = "ECO"
        '    Dim paymentId As Integer = 20
        '    Dim dueDate As DateTime = rootS.GetProperty("date_created").ToString
        '    Dim isReceivedPayment As Boolean = 0
        '    Dim currencyId As String = "ARG"

        '    CompanyIdNumer = dniOrCuit
        '    If buy.Buyer.BillingInfo.Identification.Type = "CUIT" Then
        '        lastName = firstName + " " + buy.Buyer.BillingInfo.LastName
        '        firstName = ""
        '        iscompany = 1
        '    End If

        ' insercion de datos de cabecera
        'Dim dtMLCabecera As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_INSERTAR_ORDENES_MERCADOLIBRE_CABECERA",
        '        "VALLEJO",
        '        dniOrCuit,
        '        firstName,
        '        iscompany,
        '        lastName,
        '        addressLine,
        '        city,
        '        zipCode,
        '        homePhoneNumber,
        '        fiscalId,
        '        CompanyIdNumer,
        '        receiverAddressCity,
        '        receiverAddressFirstName,
        '        receiverAddressLastName,
        '        receiverAddressLine,
        '        receiverAddressZipCode,
        '        headerComment,
        '        headerCustomerId,
        '        headerCurrencyId,
        '        headerDate,
        '        headerInternalReference,
        '        headerBillingStatus,
        '        headerDeliveryType,
        '        headerFollowUpStatus,
        '        headerGiftMessageType,
        '        headerPaymentStatus,
        '        headerReturnStatus,
        '        headerShippingStatus,
        '        headerTransporter,
        '        headerStoreId,
        '        headerWareHouseId,
        '        headerSalesPersonId,
        '        headerType,
        '        amount,
        '        methodId,
        '        paymentId,
        '        dueDate,
        '        isReceivedPayment,
        '        currencyId
        '    )

        'Dim orderItems As List(Of OrderItem) = ord.OrderItems

        'For Each item In orderItems
        '        'txtRespuesta.Text = headerDate
        '        Dim reference As String = item.Item.SellerCustomField
        '        Dim storeId As String = "000013"
        '        Dim label As String = item.Item.Title.ToString
        '        Dim deliveryDate As String = headerDate.ToString("dd-MM-yyyy")
        '        Dim quantity As Integer = item.Quantity
        '        Dim netUnitPrice As Double = item.UnitPrice
        '        Dim warehouseId As String = "000013"

        '        cegid(item, storeId, warehouseId)

        '        Dim dtMLDetalle As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_INSERTAR_ORDENES_MERCADOLIBRE_DETALLE",
        '            ordenId,
        '            reference,
        '            storeId,
        '            label,
        '            deliveryDate,
        '            quantity,
        '            netUnitPrice,
        '            warehouseId
        '        )

        '    Next

        'Next

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

                insertarDetalle(item, headerDate)

            Next

        Next

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

    Sub insertarDetalle(ByVal item As OrderItem, ByVal headerDate As Date)

        Dim reference As String = item.Item.SellerCustomField
        Dim storeId As String = "000013"
        Dim label As String = item.Item.Title.ToString
        Dim deliveryDate As String = headerDate.ToString("dd-MM-yyyy")
        Dim quantity As Integer = item.Quantity
        Dim netUnitPrice As Double = item.UnitPrice
        Dim warehouseId As String = "000198"

        Dim availableQty As Decimal = cegid(item, storeId, warehouseId)

        Dim dtMLDetalle As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_INSERTAR_ORDENES_MERCADOLIBRE_DETALLE",
            ordenId,
            reference,
            storeId,
            label,
            deliveryDate,
            quantity,
            netUnitPrice,
            warehouseId
        )

    End Sub

    Function cegid(ByVal item As OrderItem, ByVal storeId As String, ByVal warehouseId As String) As Decimal

        Dim itemIdentifier As New ItemIdentifier
        Dim retailContext As New RetailContext

        ' Crear una instancia del cliente del servicio
        Dim binding = New BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)

        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName

        Dim endpoint As New EndpointAddress("http://cegid.sportotal.com.ar/Y2_VAL/ItemInventoryWcfService.svc?singleWsdl")

        Dim clientCegid As New ItemInventoryWcfServiceClient(binding, endpoint)

        ' Establecer las credenciales
        clientCegid.ClientCredentials.UserName.UserName = "VATEST\MATIAS"
        clientCegid.ClientCredentials.UserName.Password = "MATIAS2020"

        retailContext.DatabaseId = "VATEST"

        itemIdentifier.Id = item.Item.SellerCustomField
        itemIdentifier.Reference = item.Item.SellerCustomField

        Try

            Dim resp = clientCegid.GetAvailableQty(item.Item.Id.ToString, itemIdentifier, storeId.ToString, warehouseId.ToString, retailContext)
            Return resp.AvailableQty
            'MsgBox(resp.AvailableQty)

        Catch ex As Exception

        End Try

    End Function

End Class



