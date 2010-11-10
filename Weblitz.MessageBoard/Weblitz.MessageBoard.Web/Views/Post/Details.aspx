<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Web.Models.PostDetail>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Post Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="post">
        <h2><%:Model.TopicTitle%></h2>
        <div class="body"><%:Model.Body%></div>
        <div class="meta">
            <div class="forum"><%:Model.ForumName%></div>
            <div class="author"><%:Model.Author%></div>
            <div class="date"><%:Model.PublishedDate%></div>
        </div>
        <ul class="options">
            <li><%=Html.ActionLink("Edit", "Edit", new {Model.Id})%></li>
            <li><%=Html.ActionLink("Delete", "Delete", new {Model.Id})%></li>
            <li><%=Html.ActionLink("Reply", "Create", "Post", new {TopicId = Model.Id, ParentId = Model.Id}, null)%></li>
            <li>
            <%
            using (Html.BeginForm("Flag", "Post", new {Model.Id}))
            {%>
                <input type="submit" value="Flag" />
            <%
            }%>
            </li>
        </ul>
    </div>
</asp:Content>
