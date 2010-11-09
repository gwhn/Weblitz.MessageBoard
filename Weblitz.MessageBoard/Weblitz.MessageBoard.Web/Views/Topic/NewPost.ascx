<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Weblitz.MessageBoard.Web.Models.PostInput>" %>

<div class="newpost">
    <%
        Html.EnableClientValidation();%>
    <%
        using (Html.BeginForm("Create", "Post", new {Model.TopicId}))
        {%>
    <fieldset>
        <%=Html.EditorForModel()%>
        <ul class="options">
            <li><input type="submit" value="Post" /></li>
        </ul>
    </fieldset>  
    <%
        }%>
</div>
