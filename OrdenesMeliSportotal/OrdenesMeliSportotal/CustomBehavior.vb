Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Description
Imports System.ServiceModel.Dispatcher

Public Class CustomBehavior
    Implements IClientMessageInspector, IEndpointBehavior

    Public Sub AfterReceiveReply(ByRef reply As Message, correlationState As Object) Implements IClientMessageInspector.AfterReceiveReply
        ' No necesitas hacer nada aquí
    End Sub

    Public Function BeforeSendRequest(ByRef request As Message, channel As IClientChannel) As Object Implements IClientMessageInspector.BeforeSendRequest
        ' Agregar el encabezado Content-Type
        Dim httpRequest As HttpRequestMessageProperty = Nothing
        If request.Properties.ContainsKey(HttpRequestMessageProperty.Name) Then
            httpRequest = CType(request.Properties(HttpRequestMessageProperty.Name), HttpRequestMessageProperty)
        Else
            httpRequest = New HttpRequestMessageProperty()
            request.Properties.Add(HttpRequestMessageProperty.Name, httpRequest)
        End If
        httpRequest.Headers("Content-Type") = "text/xml; charset=utf-8"
        Return Nothing
    End Function

    Public Sub AddBindingParameters(endpoint As ServiceEndpoint, bindingParameters As BindingParameterCollection) Implements IEndpointBehavior.AddBindingParameters
        ' No necesitas hacer nada aquí
    End Sub

    Public Sub ApplyClientBehavior(endpoint As ServiceEndpoint, clientRuntime As ClientRuntime) Implements IEndpointBehavior.ApplyClientBehavior
        clientRuntime.MessageInspectors.Add(New CustomBehavior())
    End Sub

    Public Sub ApplyDispatchBehavior(endpoint As ServiceEndpoint, endpointDispatcher As EndpointDispatcher) Implements IEndpointBehavior.ApplyDispatchBehavior
        ' No necesitas hacer nada aquí
    End Sub

    Public Sub Validate(endpoint As ServiceEndpoint) Implements IEndpointBehavior.Validate
        ' No necesitas hacer nada aquí
    End Sub
End Class