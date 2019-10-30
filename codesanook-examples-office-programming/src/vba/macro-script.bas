' Your code must inside Sub or Function
Sub ShowMessage()
    Dim value
    value = "Codesanook Powered by Thep Admin Pong" 

    MsgBox value
    Sheets("primary").range("a1").value = value

    ' Delay in milliseconds  
    Application.Wait (Now + TimeValue("0:00:05"))  
    Sheets("primary").range("a1").clear
End Sub
