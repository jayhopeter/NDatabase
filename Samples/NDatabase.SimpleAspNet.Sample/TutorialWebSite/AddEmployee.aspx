<%@ Page Title="NDatabase Tutorial" Language="C#" MasterPageFile="Site.master"
    AutoEventWireup="true" CodeBehind="AddEmployee.aspx.cs" Inherits="TutorialWebSite.AddEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h2>
        Add New Employee</h2>
    <table>
        <tr>
            <td>
                Name:
            </td>
            <td>
                <asp:TextBox ID="Name" runat="server" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorForName" ControlToValidate="Name"
                    Text="*" ErrorMessage="Name cannot be empty!" ForeColor="Red" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                City:
            </td>
            <td>
                <asp:TextBox ID="City" runat="server" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorForCity" ControlToValidate="City"
                    Text="*" ErrorMessage="City cannot be empty!" ForeColor="Red" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Age:
            </td>
            <td>
                <asp:TextBox ID="Age" runat="server" />
            </td>
            <td>
                <asp:RangeValidator ID="RangeValidatorForAge" ControlToValidate="Age" MinimumValue="18"
                    MaximumValue="67" Type="Integer" Text="*" ErrorMessage="The age must be from 18 to 67!"
                    ForeColor="Red" runat="server" />
            </td>
        </tr>
    </table>
    <p>
        Employment Date:
        <br />
        <asp:Calendar ID="EmploymentDate" runat="server" />
        <br />
        <br />
        <asp:Button Text="Add Employee" ID="AddButton" runat="server" OnClick="AddButton_Click" />
        <br />
        <br />
        <asp:ValidationSummary ID="ValidationSummary1" HeaderText="You have to fix the following errors:"
            DisplayMode="BulletList" EnableClientScript="true" runat="server" />
    </p>
</asp:Content>
