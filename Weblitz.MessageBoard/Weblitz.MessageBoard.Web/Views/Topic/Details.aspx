<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Web.Models.TopicDetail>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Topic Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="topic">
        <h2><%:Model.Title%></h2>
        <div class="body"><%:Model.Body%></div>
        <div class="meta">
            <div class="forum"><%:Model.Forum%></div>
            <div class="author"><%:Model.Author%></div>
            <div class="date"><%:Model.PublishedDate%></div>
        </div>
        <ul class="options">
            <li><%=Html.ActionLink("Edit", "Edit", new {Model.Id})%></li>
            <li><%=Html.ActionLink("Delete", "Delete", new {Model.Id})%></li>
        </ul>
    </div>
    <div class="postslist">
        <h3>Posts</h3>
        <%
            foreach (var post in Model.Posts)
            {%>
        <div class="post">
            <div class="body"><%:post.Body%></div>
            <div class="meta">
                <div class="author"><%:post.Author%></div>
                <div class="date"><%:post.PublishedDate%></div>
            </div>
            <ul class="options">
                <li><%=Html.ActionLink("Edit", "Edit", "Post", new {post.Id}, null)%></li>
                <li><%=Html.ActionLink("Delete", "Delete", "Post", new {post.Id}, null)%></li>
                <li>
                <%
                using (Html.BeginForm("Flag", "Post", new {post.Id}))
                {%>
                    <input type="submit" value="Flag" />
                <%
                }%>
                </li>
            </ul>
        </div>  
        <%
            }%>
    </div>
    <%
            Html.RenderPartial("NewPost", Model.NewPost);%>
</asp:Content>
