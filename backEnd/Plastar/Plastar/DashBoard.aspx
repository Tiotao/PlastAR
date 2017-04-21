<%@ Page Title="DashBoard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="Plastar.DashBoard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>This is where your casts are placed.</h3>
    <p>You can add more cast here.</p>
    <asp:Button ID="AddBundle" runat="server" Text="Add"/>
    <br>
    <asp:PlaceHolder ID="holder" runat="server" />
</asp:Content>