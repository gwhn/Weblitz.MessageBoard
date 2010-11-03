<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Web.Models.ForumSummary[]>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Forum Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <thead>
            <tr>
                <th>Forum</th>
                <th>Topic Count</th>
                <th>Post Count</th>
            </tr>
        </thead>
        <tbody>
        <%
            foreach (var forum in Model)
            {%>
            <tr>
                <td><%=Html.ActionLink(forum.Name, "Details", new {forum.Id})%></td>
                <td><%:forum.TopicCount%></td>
                <td><%:forum.PostCount%></td>
            </tr>
        <%
            }%>
        </tbody>
    </table>
    <ul class="options">
        <li><%=Html.ActionLink("New Forum", "Create")%></li>
    </ul>
</asp:Content>
