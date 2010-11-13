<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Weblitz.MessageBoard.Web.Models.PostDetail[]>" %>

<div class="postslist">
    <h3>Posts</h3>
    <%
        if (Model.Length < 1)
        {%>
    <div class="emptylist">There are no posts</div>
    <%
        }%>
    <%
        foreach (var post in Model)
        {
            Html.RenderPartial("Post", post);
        }%>
</div>
