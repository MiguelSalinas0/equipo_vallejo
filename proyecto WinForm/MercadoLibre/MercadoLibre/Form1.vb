Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Data




Public Class Form1

    Dim employeeId As String = "31366430"
    Dim nowTime As String = "2024-06-19T13:10:00.010Z"
    Dim comment As String = "suc20"


    Private Async Function ClockOutAsync() As Task
        Dim apiUrl As String = "https://api-prod.humand.co/public/api/v1/time-tracking/entries/clockin"
        Dim apiKey As String = "g3N9Rzmi3yn3JHZ6Ip6MBn1LlKZ0Zm3h"
        Dim employeeId As String = "31366430"
        Dim nowTime As String = "2024-06-19T12:45:00.010Z"
        Dim comment As String = "suc20"

        Using client As New HttpClient()
            ' Configurar encabezados
            client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", apiKey)
            client.DefaultRequestHeaders.Accept.Add(New System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"))

            ' Construir el cuerpo del mensaje
            Dim jsonBody As String = $"{{ ""employeeId"": ""{employeeId}"", ""now"": ""{nowTime}"", ""comment"": ""{comment}"" }}"

            Dim content As New StringContent(jsonBody, Encoding.UTF8, "application/json")

            ' Realizar la solicitud POST
            Dim response As HttpResponseMessage = Await client.PostAsync(apiUrl, content)

            ' Procesar la respuesta
            If response.IsSuccessStatusCode Then
                Dim responseContent As String = Await response.Content.ReadAsStringAsync()


                ' Deserializar el JSON
                Dim jsonObject As Object = JsonConvert.DeserializeObject(responseContent)

                ' Crear o llenar DataTable
                Dim dt As DataTable = CreateDataTable()

                ' Agregar fila al DataTable
                Dim newRow As DataRow = dt.NewRow()
                newRow("type") = jsonObject("type").ToString()
                newRow("userId") = Integer.Parse(jsonObject("userId").ToString())
                newRow("time") = DateTime.Parse(jsonObject("time").ToString())
                'newRow("siteId") = 
                ' newRow("location") = ""
                newRow("createdBy") = Integer.Parse(jsonObject("createdBy").ToString())
                newRow("source") = jsonObject("source").ToString()
                newRow("summaryId") = Integer.Parse(jsonObject("summaryId").ToString())
                newRow("comment") = jsonObject("comment").ToString()
                newRow("id") = Integer.Parse(jsonObject("id").ToString())
                dt.Rows.Add(newRow)

                ' Mostrar DataTable en DataGridView (opcional)
                dgvResult.DataSource = dt

            Else
                Dim errorMessage As String = $"Error al hacer la solicitud: {response.StatusCode} - {response.ReasonPhrase}"
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Using
    End Function

    Private Function CreateDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("type", GetType(String))
        dt.Columns.Add("userId", GetType(Integer))
        dt.Columns.Add("time", GetType(DateTime))
        dt.Columns.Add("siteId", GetType(Integer))
        dt.Columns.Add("location", GetType(String))
        dt.Columns.Add("createdBy", GetType(Integer))
        dt.Columns.Add("source", GetType(String))
        dt.Columns.Add("summaryId", GetType(Integer))
        dt.Columns.Add("comment", GetType(String))
        dt.Columns.Add("id", GetType(Integer))
        Return dt
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click

        ClockOutAsync()

    End Sub


    Public Class Rootobject
        Public Property type As String
        Public Property userId As Integer
        Public Property time As Date
        Public Property siteId As Object
        Public Property location As Object
        Public Property createdBy As Integer
        Public Property source As String
        Public Property summaryId As Integer
        Public Property comment As String
        Public Property id As Integer
    End Class


End Class
