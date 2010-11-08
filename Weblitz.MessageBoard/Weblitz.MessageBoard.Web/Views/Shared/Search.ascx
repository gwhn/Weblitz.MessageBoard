<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Weblitz.MessageBoard.Web.Models.SearchInput>" %>

<%
    Html.EnableClientValidation();%>
<%
    using (Html.BeginForm())
    {%>
    <%=Html.TextBoxFor(m => m.Query)%>
    <input type="submit" value="Search" />
    <%=Html.ValidationMessageFor(m => m.Query)%>
<%
    }%>
