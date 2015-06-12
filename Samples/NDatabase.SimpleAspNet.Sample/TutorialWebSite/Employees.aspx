<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="TutorialWebSite.Employees" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="MainContent">
    <h2>
        List of Employees</h2>
    <p>
        <asp:GridView ID="EmployeesGridView" runat="server" />
    </p>
</asp:Content>
