Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters


Partial Public Class ClienteMl
        <JsonProperty("site_id")>
        Public Property SiteId As String
        <JsonProperty("buyer")>
        Public Property Buyer As Buyer
        <JsonProperty("seller")>
        Public Property Seller As Seller
    End Class

    Partial Public Class Buyer
        <JsonProperty("cust_id")>
        <JsonConverter(GetType(PurpleParseStringConverter))>
        Public Property CustId As Long
        <JsonProperty("billing_info")>
        Public Property BillingInfo As BillingInfo
    End Class

    Partial Public Class BillingInfo
        <JsonProperty("name")>
        Public Property Name As String
        <JsonProperty("last_name", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property LastName As String
        <JsonProperty("identification")>
        Public Property Identification As Identification
        <JsonProperty("taxes")>
        Public Property Taxes As Taxes
        <JsonProperty("address")>
        Public Property Address As Address
        <JsonProperty("attributes")>
        Public Property Attributes As Attributes
    End Class

    Partial Public Class Address
        <JsonProperty("street_name")>
        Public Property StreetName As String
        <JsonProperty("street_number")>
        <JsonConverter(GetType(PurpleParseStringConverter))>
        Public Property StreetNumber As Long
        <JsonProperty("city_name")>
        Public Property CityName As String
        <JsonProperty("state")>
        Public Property State As State
        <JsonProperty("zip_code")>
        <JsonConverter(GetType(PurpleParseStringConverter))>
        Public Property ZipCode As Long
        <JsonProperty("country_id")>
        Public Property CountryId As String
    End Class

    Partial Public Class State
        <JsonProperty("code")>
        Public Property Code As String
        <JsonProperty("name")>
        Public Property Name As String
    End Class

    Partial Public Class Attributes
        <JsonProperty("vat_discriminated_billing")>
        <JsonConverter(GetType(FluffyParseStringConverter))>
        Public Property VatDiscriminatedBilling As Boolean
        <JsonProperty("doc_type_number")>
        <JsonConverter(GetType(PurpleParseStringConverter))>
        Public Property DocTypeNumber As Long
        <JsonProperty("is_normalized")>
        Public Property IsNormalized As Boolean
        <JsonProperty("cust_type")>
        Public Property CustType As String
    End Class

    Partial Public Class Identification
        <JsonProperty("type")>
        Public Property Type As String
        <JsonProperty("number")>
        <JsonConverter(GetType(PurpleParseStringConverter))>
        Public Property Number As Long
    End Class

    Partial Public Class Taxes
        <JsonProperty("taxpayer_type")>
        Public Property TaxpayerType As TaxpayerType
    End Class

    Partial Public Class TaxpayerType
        <JsonProperty("id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Id As String
        <JsonProperty("description")>
        Public Property Description As String
    End Class

    Partial Public Class Seller
        <JsonProperty("cust_id")>
        Public Property CustId As Long
    End Class


Partial Public Class ClienteMl
    Public Shared Function FromJson(json As String) As ClienteMl()
        Return JsonConvert.DeserializeObject(Of ClienteMl())(json, Converter.Settings)
    End Function
End Class

Public Module Serialize
    <System.Runtime.CompilerServices.Extension>
    Public Function ToJson(self As ClienteMl()) As String
        Return JsonConvert.SerializeObject(self, Converter.Settings)
    End Function
End Module

Friend Module Converter
    Public ReadOnly Settings As JsonSerializerSettings = New JsonSerializerSettings With {
        .MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        .DateParseHandling = DateParseHandling.None,
        .Converters = {New IsoDateTimeConverter With {.DateTimeStyles = DateTimeStyles.AssumeUniversal}}
    }
End Module

Friend Class PurpleParseStringConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(t As Type) As Boolean
        Return t Is GetType(Long) OrElse t Is GetType(Long?)
    End Function

    Public Overrides Function ReadJson(reader As JsonReader, t As Type, existingValue As Object, serializer As JsonSerializer) As Object
        Try
            If reader.TokenType = JsonToken.Null Then
                Return Nothing
            End If
            Dim value = serializer.Deserialize(Of String)(reader)
            Dim l As Long
            If Int64.TryParse(value, l) Then
                Return l
            End If
            'Throw New Exception("Cannot unmarshal type long")
        Catch ex As Exception

        End Try

    End Function

    Public Overrides Sub WriteJson(writer As JsonWriter, untypedValue As Object, serializer As JsonSerializer)
        If untypedValue Is Nothing Then
            serializer.Serialize(writer, Nothing)
            Return
        End If
        Dim value As Long = CType(untypedValue, Long)
        serializer.Serialize(writer, value.ToString())
        Return
    End Sub

    Public Shared ReadOnly Singleton As PurpleParseStringConverter = New PurpleParseStringConverter()
End Class

Friend Class FluffyParseStringConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(t As Type) As Boolean
        Return t Is GetType(Boolean) OrElse t Is GetType(Boolean?)
    End Function

    Public Overrides Function ReadJson(reader As JsonReader, t As Type, existingValue As Object, serializer As JsonSerializer) As Object
        If reader.TokenType = JsonToken.Null Then
            Return Nothing
        End If
        Dim value = serializer.Deserialize(Of String)(reader)
        Dim b As Boolean
        If Boolean.TryParse(value, b) Then
            Return b
        End If
        Throw New Exception("Cannot unmarshal type bool")
    End Function

    Public Overrides Sub WriteJson(writer As JsonWriter, untypedValue As Object, serializer As JsonSerializer)
        If untypedValue Is Nothing Then
            serializer.Serialize(writer, Nothing)
            Return
        End If
        Dim value As Boolean = CType(untypedValue, Boolean)
        Dim boolString As String = If(value, "true", "false")
        serializer.Serialize(writer, boolString)
        Return
    End Sub

    Public Shared ReadOnly Singleton As FluffyParseStringConverter = New FluffyParseStringConverter()
End Class