Imports System
Imports STJS = System.Text.Json.Serialization
Imports NJ = Newtonsoft.Json
Imports Newtonsoft.Json


Partial Public Class Producto
    <NJ.JsonProperty("payments")>
    Public Property Payments As List(Of Payment)
    <NJ.JsonProperty("fulfilled")>
    Public Property Fulfilled As Object
    <NJ.JsonProperty("taxes")>
    Public Property Taxes As Taxes
    <NJ.JsonProperty("order_request")>
    Public Property OrderRequest As OrderRequest
    <NJ.JsonProperty("expiration_date")>
    Public Property ExpirationDate As DateTimeOffset
    <NJ.JsonProperty("feedback")>
    Public Property Feedback As Feedback
    <NJ.JsonProperty("shipping")>
    Public Property Shipping As Shipping
    <NJ.JsonProperty("date_closed")>
    Public Property DateClosed As DateTimeOffset
    <NJ.JsonProperty("id")>
    Public Property Id As Long
    <NJ.JsonProperty("manufacturing_ending_date")>
    Public Property ManufacturingEndingDate As Object
    <NJ.JsonProperty("order_items")>
    Public Property OrderItems As OrderItem()
    <NJ.JsonProperty("date_last_updated")>
    Public Property DateLastUpdated As DateTimeOffset
    <NJ.JsonProperty("last_updated")>
    Public Property LastUpdated As DateTimeOffset
    <NJ.JsonProperty("comment")>
    Public Property Comment As Object
    <NJ.JsonProperty("pack_id")>
    Public Property PackId As Object
    <NJ.JsonProperty("coupon")>
    Public Property Coupon As Coupon
    <NJ.JsonProperty("shipping_cost")>
    Public Property ShippingCost As Object
    <NJ.JsonProperty("date_created")>
    Public Property DateCreated As DateTimeOffset
    <NJ.JsonProperty("pickup_id")>
    Public Property PickupId As Object
    <NJ.JsonProperty("status_detail")>
    Public Property StatusDetail As Object
    <NJ.JsonProperty("tags")>
    Public Property Tags As String()
    <NJ.JsonProperty("buyer")>
    Public Property Buyer As Buyer
    <NJ.JsonProperty("seller")>
    Public Property Seller As Buyer
    <NJ.JsonProperty("total_amount")>
    Public Property TotalAmount As Long
    <NJ.JsonProperty("paid_amount")>
    Public Property PaidAmount As Long
    <NJ.JsonProperty("currency_id")>
    Public Property CurrencyId As String
    <NJ.JsonProperty("status")>
    Public Property Status As String
    <NJ.JsonProperty("context")>
    Public Property Context As Context
End Class

Partial Public Class Buyer
    <NJ.JsonProperty("id")>
    Public Property Id As Long
    <NJ.JsonProperty("nickname")>
    Public Property Nickname As String
End Class

Partial Public Class Context
    <NJ.JsonProperty("application")>
    Public Property Application As Object
    <NJ.JsonProperty("product_id")>
    Public Property ProductId As Object
    <NJ.JsonProperty("channel")>
    Public Property Channel As String
    <NJ.JsonProperty("site")>
    Public Property Site As String
    <NJ.JsonProperty("flows")>
    Public Property Flows As Object()
End Class

Partial Public Class Coupon
    <NJ.JsonProperty("amount")>
    Public Property Amount As Long
    <NJ.JsonProperty("id")>
    Public Property Id As Object
End Class

Partial Public Class Feedback
    <NJ.JsonProperty("buyer")>
    Public Property Buyer As Object
    <NJ.JsonProperty("seller")>
    Public Property Seller As Object
End Class

Partial Public Class OrderItem
    <NJ.JsonProperty("item")>
    Public Property Item As Item
    <NJ.JsonProperty("quantity")>
    Public Property Quantity As Long
    <NJ.JsonProperty("unit_price")>
    Public Property UnitPrice As Long
    <NJ.JsonProperty("full_unit_price")>
    Public Property FullUnitPrice As Long
    <NJ.JsonProperty("currency_id")>
    Public Property CurrencyId As String
    <NJ.JsonProperty("manufacturing_days")>
    Public Property ManufacturingDays As Object
    <NJ.JsonProperty("picked_quantity")>
    Public Property PickedQuantity As Object
    <NJ.JsonProperty("requested_quantity")>
    Public Property RequestedQuantity As RequestedQuantity
    <NJ.JsonProperty("sale_fee")>
    Public Property SaleFee As Long
    <NJ.JsonProperty("listing_type_id")>
    Public Property ListingTypeId As String
    <NJ.JsonProperty("base_exchange_rate")>
    Public Property BaseExchangeRate As Object
    <NJ.JsonProperty("base_currency_id")>
    Public Property BaseCurrencyId As Object
    <NJ.JsonProperty("bundle")>
    Public Property Bundle As Object
    <NJ.JsonProperty("element_id")>
    Public Property ElementId As Long
End Class

Partial Public Class Item
    <NJ.JsonProperty("id")>
    Public Property Id As String
    <NJ.JsonProperty("title")>
    Public Property Title As String
    <NJ.JsonProperty("category_id")>
    Public Property CategoryId As String
    <NJ.JsonProperty("variation_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property VariationId As Long
    <NJ.JsonProperty("seller_custom_field")>
    Public Property SellerCustomField As String
    <NJ.JsonProperty("global_price")>
    Public Property GlobalPrice As Object
    <NJ.JsonProperty("net_weight")>
    Public Property NetWeight As Object
    <NJ.JsonProperty("variation_attributes")>
    Public Property VariationAttributes As VariationAttribute()
    <NJ.JsonProperty("warranty")>
    Public Property Warranty As Object
    <NJ.JsonProperty("condition")>
    Public Property Condition As String
    <NJ.JsonProperty("seller_sku")>
    Public Property SellerSku As String
End Class

Partial Public Class VariationAttribute
    <NJ.JsonProperty("name")>
    Public Property Name As String
    <NJ.JsonProperty("id")>
    Public Property Id As String
    <NJ.JsonProperty("value_id")>
    <NJ.JsonConverter(GetType(ParseStringConverter))>
    Public Property ValueId As Long
    <NJ.JsonProperty("value_name")>
    Public Property ValueName As String

End Class

Partial Public Class RequestedQuantity
    <NJ.JsonProperty("measure")>
    Public Property Measure As String
    <NJ.JsonProperty("value")>
    Public Property Value As Long
End Class

Partial Public Class OrderRequest
    <NJ.JsonProperty("change")>
    Public Property Change As Object
    <NJ.JsonProperty("return")>
    Public Property [Return] As Object
End Class

Partial Public Class Payment
    <NJ.JsonProperty("reason")>
    Public Property Reason As String
    <NJ.JsonProperty("status_code")>
    Public Property StatusCode As Object
    <NJ.JsonProperty("total_paid_amount")>
    Public Property TotalPaidAmount As Long
    <NJ.JsonProperty("operation_type")>
    Public Property OperationType As String
    <NJ.JsonProperty("transaction_amount")>
    Public Property TransactionAmount As Long
    <NJ.JsonProperty("transaction_amount_refunded")>
    Public Property TransactionAmountRefunded As Long
    <NJ.JsonProperty("date_approved", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property DateApproved As DateTimeOffset
    <NJ.JsonProperty("collector")>
    Public Property Collector As Shipping
    <NJ.JsonProperty("coupon_id")>
    Public Property CouponId As Object
    <NJ.JsonProperty("installments")>
    Public Property Installments As Long
    <NJ.JsonProperty("authorization_code")>
    Public Property AuthorizationCode As String
    <NJ.JsonProperty("taxes_amount")>
    Public Property TaxesAmount As Long
    <NJ.JsonProperty("id")>
    Public Property Id As Long
    <NJ.JsonProperty("date_last_modified")>
    Public Property DateLastModified As DateTimeOffset
    <NJ.JsonProperty("coupon_amount")>
    Public Property CouponAmount As Long
    <NJ.JsonProperty("available_actions")>
    Public Property AvailableActions As String()
    <NJ.JsonProperty("shipping_cost")>
    Public Property ShippingCost As Long
    <NJ.JsonProperty("installment_amount", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property InstallmentAmount As Double
    <NJ.JsonProperty("date_created")>
    Public Property DateCreated As DateTimeOffset
    <NJ.JsonProperty("activation_uri")>
    Public Property ActivationUri As Object
    <NJ.JsonProperty("overpaid_amount")>
    Public Property OverpaidAmount As Long
    <NJ.JsonProperty("card_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property CardId As Long
    <NJ.JsonProperty("status_detail")>
    Public Property StatusDetail As String
    <NJ.JsonConverter(GetType(ParseStringConverter))>
    Public Property IssuerId As Long
    <NJ.JsonProperty("payment_method_id")>
    Public Property PaymentMethodId As String
    <NJ.JsonProperty("payment_type")>
    Public Property PaymentType As String
    <NJ.JsonProperty("deferred_period")>
    Public Property DeferredPeriod As Object
    <NJ.JsonProperty("atm_transfer_reference")>
    Public Property AtmTransferReference As AtmTransferReference
    <NJ.JsonProperty("site_id")>
    Public Property SiteId As String
    <NJ.JsonProperty("payer_id")>
    Public Property PayerId As Long
    <NJ.JsonProperty("order_id")>
    Public Property OrderId As Long
    <NJ.JsonProperty("currency_id")>
    Public Property CurrencyId As String
    <NJ.JsonProperty("status")>
    Public Property Status As String
    <NJ.JsonProperty("transaction_order_id")>
    Public Property TransactionOrderId As Object
End Class

Partial Public Class AtmTransferReference
    <NJ.JsonProperty("transaction_id")>
    Public Property TransactionId As String
    <NJ.JsonProperty("company_id")>
    Public Property CompanyId As Object
End Class

Partial Public Class Shipping
    <NJ.JsonProperty("id")>
    Public Property Id As Long
End Class

Partial Public Class Taxes
    <NJ.JsonProperty("amount")>
    Public Property Amount As Object
    <NJ.JsonProperty("currency_id")>
    Public Property CurrencyId As Object
    <NJ.JsonProperty("id")>
    Public Property Id As Object
End Class



Friend Class ParseStringConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(ByVal t As Type) As Boolean
        Return t = GetType(Long) OrElse t = GetType(Long?)
    End Function

    Public Overrides Function ReadJson(ByVal reader As JsonReader, ByVal t As Type, ByVal existingValue As Object, ByVal serializer As JsonSerializer) As Object
        If reader.TokenType = JsonToken.Null Then Return Nothing
        Dim value = serializer.Deserialize(Of String)(reader)
        Dim l As Long

        If Int64.TryParse(value, l) Then
            Return l
        End If

        Throw New Exception("Cannot unmarshal type long")
    End Function

    Public Overrides Sub WriteJson(ByVal writer As JsonWriter, ByVal untypedValue As Object, ByVal serializer As JsonSerializer)
        If untypedValue Is Nothing Then
            serializer.Serialize(writer, Nothing)
            Return
        End If

        Dim value = CLng(untypedValue)
        serializer.Serialize(writer, value.ToString())
        Return
    End Sub

    Public Shared ReadOnly Singleton As ParseStringConverter = New ParseStringConverter()
End Class
