<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Web.Models.ForumDetail>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Forum Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:Model.Name%></h2>
    <table>
        <thead>
            <tr>
                <th>Title</th>
                <th>Post Count</th>
                <th>Sticky</th>
            </tr>
        </thead>
        <tbody>
        <%
            foreach (var topic in Model.Topics)
            {%>
            <tr>
                <td><%=Html.ActionLink(topic.Title, "Details", "Topic", new {topic.Id}, null)%></td>
                <td><%:topic.PostCount%></td>
                <td>
                <%
                    if (topic.Sticky)
                    {%>
                    <img src="/Content/Images/Sticky.jpg" alt="Sticky Topic" width="32" height="30" />
                <%
                    }%>
                </td>
            </tr>  
        <%
            }%>
        </tbody>
    </table>
    <ul class="options">
        <li><%=Html.ActionLink("Edit", "Edit", new { Model.Id }, null)%></li>
        <li><%=Html.ActionLink("Delete", "Delete", new { Model.Id }, null)%></li>
        <li><%=Html.ActionLink("New Topic", "Create", "Topic", new {ForumId = Model.Id}, null)%></li>
    </ul>
</asp:Content>
