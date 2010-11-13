<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Web.Models.DeleteItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete Topic
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("DeleteConfirmation"); %>
</asp:Content>
