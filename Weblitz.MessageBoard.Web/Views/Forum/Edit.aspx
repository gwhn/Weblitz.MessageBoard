<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Web.Models.ForumInput>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit <%:Model.Name%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit <%:Model.Name%></h2>
    <%
        Html.RenderPartial("Form");%>
</asp:Content>
