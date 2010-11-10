<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Web.Models.TopicDetail>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Topic Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="topic">
        <h2><%:Model.Title%></h2>
        <div class="body"><%:Model.Body%></div>
        <div class="meta">
            <div class="forum"><%:Model.ForumName%></div>
            <div class="author"><%:Model.Author%></div>
            <div class="date"><%:Model.PublishedDate%></div>
        </div>
        <ul class="options">
            <li><%=Html.ActionLink("Edit", "Edit", new {Model.Id})%></li>
            <li><%=Html.ActionLink("Delete", "Delete", new {Model.Id})%></li>
            <li><%=Html.ActionLink("New Post", "Create", "Post", new {TopicId = Model.Id}, null)%></li>
        </ul>
    </div>
    <div class="postslist">
        <h3>Posts</h3>
        <%
            foreach (var post in Model.Posts)
            {
                Html.RenderPartial("Post", post);
            }%>
    </div>
</asp:Content>
