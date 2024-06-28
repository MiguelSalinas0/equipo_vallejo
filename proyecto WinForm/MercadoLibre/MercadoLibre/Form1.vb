Imports System.Net.Http
Imports Microsoft.Identity
Imports System.Text.Json
Imports Newtonsoft.Json
Imports MercadoLibre.OrdenML

Public Class Form1
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Establecer la conexion a DB 
        Dim dtSucs As DataTable

        dtSucs = ConexionBBDD.ConexionSQL.EjecutarSP("sp_obtener_sucursales")

        cmbSucursales.DataSource = dtSucs
        cmbSucursales.DisplayMember = "Sucursal"

        Dim dtToken As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP_SAP("SP_OBTENER_TOKEN", 1)
        Dim token As String = dtToken.Rows(0).Item(0)
        ' FALTA CONEXION A w1_pueblo

        'Datos ML
        Dim apiMLBase As String = "https://api.mercadolibre.com/"
        Dim userId As Integer = 257128833
        Dim clientId As String = "5038796649356510"
        Dim clientSecret As String = "SSP5q4xPndDRru0ut4FelVTg5BrUXRu7"

        Dim dateFrom As String = Date.Now.AddDays(-2).Date.ToString("yyyy-MM-dd") + "T00:00:00"
        Dim dateTo As String = Date.Now.AddDays(1).Date.ToString("yyyy-MM-dd") + "T00:00:00"

        Dim url As String = apiMLBase + "orders/search?seller=" + userId.ToString + "&order.status=paid&order.date_created.from=" + dateFrom + ".000-00:00&order.date_created.to=" + dateTo + ".000-00:00"

        Dim httpClient = New HttpClient()
        ' Dim token = "APP_USR-5038796649356510-062406-93f2014b22c4a27f9ace18498e7bb360-257128833"

        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token)

        Dim respuesta As HttpResponseMessage = Await httpClient.GetAsync(url)

        Dim body = Await respuesta.Content.ReadAsStringAsync
        '' txtRespuesta.Text = body

        Dim jsonDoc As JsonDocument = JsonDocument.Parse(body)
        Dim root As JsonElement = jsonDoc.RootElement


        '' formatear y mapear la respuesta
        txtRespuesta.Text = root.GetProperty("results")(0).ToString

        Dim ordenes As List(Of Producto) = JsonConvert.DeserializeObject(Of List(Of Producto))(root.GetProperty("results").ToString)
        Dim ordenId As Long
        Dim urlGetBuy As String
        Dim respuestaBuy As HttpResponseMessage
        Dim bodyBuy As String
        Dim jsonClient As JsonDocument
        Dim rootC As JsonElement
        httpClient.DefaultRequestHeaders.Add("x-version", 2)
        For Each ord In ordenes
            ordenId = ord.Id
            urlGetBuy = apiMLBase & "orders/" & ordenId & "/billing_info"
            httpClient.DefaultRequestHeaders.Add("x-format-new", True)

            ' Peticion para traer detalles del cliente a partir de una orden
            respuestaBuy = Await httpClient.GetAsync(urlGetBuy)
            bodyBuy = Await respuestaBuy.Content.ReadAsStringAsync
            jsonClient = JsonDocument.Parse(bodyBuy)
            rootC = jsonClient.RootElement
            Dim buy As ClienteMl = JsonConvert.DeserializeObject(Of ClienteMl)(rootC.ToString)

            ' Peticion para traer datos del envio a partir de una orden
            Dim urlGetshipment As String = apiMLBase & "orders/" & ordenId & "/shipments"
            Dim respuestaShip = Await httpClient.GetAsync(urlGetshipment)
            Dim bodyShip = Await respuestaShip.Content.ReadAsStringAsync
            Dim jsonShip = JsonDocument.Parse(bodyShip)
            Dim rootS = jsonShip.RootElement


            ' Definicion de parametros para SP
            Dim firstName As String = buy.Buyer.BillingInfo.Name
            Dim lastName As String = buy.Buyer.BillingInfo.LastName
            Dim iscompany As Boolean = 0
            Dim dniOrCuit As String = buy.Buyer.BillingInfo.Identification.Number
            Dim CompanyIdNumer As String
            Dim addressLine As String = buy.Buyer.BillingInfo.Address.StreetName
            Dim city As String = buy.Buyer.BillingInfo.Address.CityName
            Dim zipCode As String = buy.Buyer.BillingInfo.Address.ZipCode
            Dim homePhoneNumber As String = ""
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
            Dim headerInternalReference As String = ord.Id
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

            Dim dt As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("SP_INSERTAR_ORDENES_MERCADOLIBRE_CABECERA",
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


        Next



    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSucursales.SelectedIndexChanged

    End Sub
End Class



