<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Weblitz.MessageBoard.Web.Models.PostDetail>" %>

<div class="post" style="margin-left:<%=(Model.Level*100)%>px">
    <div class="body"><%:Model.Body%></div>
    <div class="meta">
        <div class="author"><%:Model.Author%></div>
        <div class="date"><%:Model.PublishedDate%></div>
    </div>
    <ul class="options">
        <li><%=Html.ActionLink("View", "Details", "Post", new {Model.Id}, null)%></li>
        <li><%=Html.ActionLink("Edit", "Edit", "Post", new {Model.Id}, null)%></li>
        <li><%=Html.ActionLink("Delete", "Delete", "Post", new {Model.Id}, null)%></li>
        <li><%=Html.ActionLink("Reply", "Create", "Post", new {Model.TopicId, ParentId = Model.Id}, null)%></li>
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
<%
    foreach (var child in Model.Children)
    {
        Html.RenderPartial("Post", child);
    }%>
