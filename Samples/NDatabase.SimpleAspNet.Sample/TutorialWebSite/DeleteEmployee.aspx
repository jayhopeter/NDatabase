<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="DeleteEmployee.aspx.cs" Inherits="TutorialWebSite.DeleteEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h2>
        Remove Employee</h2>
    <p>
        Name: <asp:TextBox ID="ID" runat="server" />
        <br />
        <br />
        <asp:Button Text="Remove Employee" ID="RemoveButton" runat="server" OnClick="RemoveButton_Click" />
        <br />
        <asp:RangeValidator ID="RangeValidatorForAge" ControlToValidate="ID" MinimumValue="2"
            MaximumValue="20000" Type="Integer" Text="The ID must be greater than 1 and less than 20000!"
            runat="server" />
        <br />
        <asp:Label ID="ErrorMessage" ForeColor="Red" runat="server" />
    </p>
</asp:Content>
