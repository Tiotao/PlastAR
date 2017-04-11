<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Plastar._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-4">
            <h2>Upload</h2>
            <p>
                Please upload your resource.
            </p>

             
            <input type="file" id="File1" name="File1" runat="server" />
            <br>
            <input type="submit" id="Submit1" value="Upload" runat="server" />
            <br>
            <br>
            <asp:Button ID="Build1" runat="server" Text="Build" />
        </div>
    </div>

</asp:Content>
