Imports Newtonsoft.Json

Partial Public Class Producto
    <JsonProperty("payments")>
    Public Property Payments As List(Of Payment)
    <JsonProperty("fulfilled")>
    Public Property Fulfilled As Object
    <JsonProperty("taxes")>
    Public Property Taxes As Taxes
    <JsonProperty("order_request")>
    Public Property OrderRequest As OrderRequest
    <JsonProperty("expiration_date")>
    Public Property ExpirationDate As DateTimeOffset
    <JsonProperty("feedback")>
    Public Property Feedback As Feedback
    <JsonProperty("shipping")>
    Public Property Shipping As Shipping
    <JsonProperty("date_closed")>
    Public Property DateClosed As DateTimeOffset
    <JsonProperty("id")>
    Public Property Id As Long
    <JsonProperty("manufacturing_ending_date")>
    Public Property ManufacturingEndingDate As Object
    <JsonProperty("order_items")>
    Public Property OrderItems As List(Of OrderItem)
    <JsonProperty("date_last_updated")>
    Public Property DateLastUpdated As DateTimeOffset
    <JsonProperty("last_updated")>
    Public Property LastUpdated As DateTimeOffset
    <JsonProperty("comment")>
    Public Property Comment As Object
    <JsonProperty("pack_id")>
    Public Property PackId As Object
    <JsonProperty("coupon")>
    Public Property Coupon As Coupon
    <JsonProperty("shipping_cost")>
    Public Property ShippingCost As Object
    <JsonProperty("date_created")>
    Public Property DateCreated As DateTimeOffset
    <JsonProperty("pickup_id")>
    Public Property PickupId As Object
    <JsonProperty("status_detail")>
    Public Property StatusDetail As Object
    <JsonProperty("tags")>
    Public Property Tags As String()
    <JsonProperty("buyer")>
    Public Property Buyer As Buyer
    <JsonProperty("seller")>
    Public Property Seller As Buyer
    <JsonProperty("total_amount")>
    Public Property TotalAmount As Long
    <JsonProperty("paid_amount")>
    Public Property PaidAmount As Long
    <JsonProperty("currency_id")>
    Public Property CurrencyId As String
    <JsonProperty("status")>
    Public Property Status As String
    <JsonProperty("context")>
    Public Property Context As Context
End Class

Partial Public Class Buyer
    <JsonProperty("id")>
    Public Property Id As Long
    <JsonProperty("nickname")>
    Public Property Nickname As String
End Class

Partial Public Class Context
    <JsonProperty("application")>
    Public Property Application As Object
    <JsonProperty("product_id")>
    Public Property ProductId As Object
    <JsonProperty("channel")>
    Public Property Channel As String
    <JsonProperty("site")>
    Public Property Site As String
    <JsonProperty("flows")>
    Public Property Flows As Object()
End Class

Partial Public Class Coupon
    <JsonProperty("amount")>
    Public Property Amount As Long
    <JsonProperty("id")>
    Public Property Id As Object
End Class

Partial Public Class Feedback
    <JsonProperty("buyer")>
    Public Property Buyer As Object
    <JsonProperty("seller")>
    Public Property Seller As Object
End Class

Partial Public Class OrderItem
    <JsonProperty("item")>
    Public Property Item As Item
    <JsonProperty("quantity")>
    Public Property Quantity As Long
    <JsonProperty("unit_price")>
    Public Property UnitPrice As Long
    <JsonProperty("full_unit_price")>
    Public Property FullUnitPrice As Long
    <JsonProperty("currency_id")>
    Public Property CurrencyId As String
    <JsonProperty("manufacturing_days")>
    Public Property ManufacturingDays As Object
    <JsonProperty("picked_quantity")>
    Public Property PickedQuantity As Object
    <JsonProperty("requested_quantity")>
    Public Property RequestedQuantity As RequestedQuantity
    <JsonProperty("sale_fee")>
    Public Property SaleFee As Long
    <JsonProperty("listing_type_id")>
    Public Property ListingTypeId As String
    <JsonProperty("base_exchange_rate")>
    Public Property BaseExchangeRate As Object
    <JsonProperty("base_currency_id")>
    Public Property BaseCurrencyId As Object
    <JsonProperty("bundle")>
    Public Property Bundle As Object
    <JsonProperty("element_id")>
    Public Property ElementId As Long
End Class

Partial Public Class Item
    <JsonProperty("id")>
    Public Property Id As String
    <JsonProperty("title")>
    Public Property Title As String
    <JsonProperty("category_id")>
    Public Property CategoryId As String
    <JsonProperty("variation_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property VariationId As Long
    <JsonProperty("seller_custom_field")>
    Public Property SellerCustomField As String
    <JsonProperty("global_price")>
    Public Property GlobalPrice As Object
    <JsonProperty("net_weight")>
    Public Property NetWeight As Object
    <JsonProperty("variation_attributes")>
    Public Property VariationAttributes As VariationAttribute()
    <JsonProperty("warranty")>
    Public Property Warranty As Object
    <JsonProperty("condition")>
    Public Property Condition As String
    <JsonProperty("seller_sku")>
    Public Property SellerSku As String
End Class

Partial Public Class VariationAttribute
    <JsonProperty("name")>
    Public Property Name As String
    <JsonProperty("id")>
    Public Property Id As String
    <JsonProperty("value_id", NullValueHandling:=NullValueHandling.Ignore)>
    <JsonConverter(GetType(ParseStringConverter))>
    Public Property ValueId As Long
    <JsonProperty("value_name")>
    Public Property ValueName As String

End Class

Partial Public Class RequestedQuantity
    <JsonProperty("measure")>
    Public Property Measure As String
    <JsonProperty("value")>
    Public Property Value As Long
End Class

Partial Public Class OrderRequest
    <JsonProperty("change")>
    Public Property Change As Object
    <JsonProperty("return")>
    Public Property [Return] As Object
End Class

Partial Public Class Payment
    <JsonProperty("reason")>
    Public Property Reason As String
    <JsonProperty("status_code")>
    Public Property StatusCode As Object
    <JsonProperty("total_paid_amount")>
    Public Property TotalPaidAmount As Long
    <JsonProperty("operation_type")>
    Public Property OperationType As String
    <JsonProperty("transaction_amount")>
    Public Property TransactionAmount As Long
    <JsonProperty("transaction_amount_refunded")>
    Public Property TransactionAmountRefunded As Long
    <JsonProperty("date_approved", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property DateApproved As DateTimeOffset
    <JsonProperty("collector")>
    Public Property Collector As Shipping
    <JsonProperty("coupon_id")>
    Public Property CouponId As Object
    <JsonProperty("installments")>
    Public Property Installments As Long
    <JsonProperty("authorization_code")>
    Public Property AuthorizationCode As String
    <JsonProperty("taxes_amount")>
    Public Property TaxesAmount As Long
    <JsonProperty("id")>
    Public Property Id As Long
    <JsonProperty("date_last_modified")>
    Public Property DateLastModified As DateTimeOffset
    <JsonProperty("coupon_amount")>
    Public Property CouponAmount As Long
    <JsonProperty("available_actions")>
    Public Property AvailableActions As String()
    <JsonProperty("shipping_cost")>
    Public Property ShippingCost As Long
    <JsonProperty("installment_amount", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property InstallmentAmount As Double
    <JsonProperty("date_created")>
    Public Property DateCreated As DateTimeOffset
    <JsonProperty("activation_uri")>
    Public Property ActivationUri As Object
    <JsonProperty("overpaid_amount")>
    Public Property OverpaidAmount As Long
    <JsonProperty("card_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property CardId As Long
    <JsonProperty("status_detail")>
    Public Property StatusDetail As String
    <JsonConverter(GetType(ParseStringConverter))>
    Public Property IssuerId As Long
    <JsonProperty("payment_method_id")>
    Public Property PaymentMethodId As String
    <JsonProperty("payment_type")>
    Public Property PaymentType As String
    <JsonProperty("deferred_period")>
    Public Property DeferredPeriod As Object
    <JsonProperty("atm_transfer_reference")>
    Public Property AtmTransferReference As AtmTransferReference
    <JsonProperty("site_id")>
    Public Property SiteId As String
    <JsonProperty("payer_id")>
    Public Property PayerId As Long
    <JsonProperty("order_id")>
    Public Property OrderId As Long
    <JsonProperty("currency_id")>
    Public Property CurrencyId As String
    <JsonProperty("status")>
    Public Property Status As String
    <JsonProperty("transaction_order_id")>
    Public Property TransactionOrderId As Object
End Class

Partial Public Class AtmTransferReference
    <JsonProperty("transaction_id")>
    Public Property TransactionId As String
    <JsonProperty("company_id")>
    Public Property CompanyId As Object
End Class

Partial Public Class Shipping
    <JsonProperty("id")>
    Public Property Id As Long
End Class

Partial Public Class Taxes
    <JsonProperty("amount")>
    Public Property Amount As Object
    <JsonProperty("currency_id")>
    Public Property CurrencyId As Object
    <JsonProperty("id")>
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

