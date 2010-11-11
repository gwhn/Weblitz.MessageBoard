<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Weblitz.MessageBoard.Web.Models.PostInput>" %>

<%
    Html.EnableClientValidation();%>
<%
    using (Html.BeginForm(new {Model.Id, Model.TopicId, Model.ParentId}))
    {%>
    <fieldset>
        <%=Html.EditorForModel()%>
        <ul class="options">
            <%
        if (Model.ParentId.HasValue)
        {%>
            <li><%=Html.ActionLink("Cancel", "Details", "Post", new {Id = Model.ParentId.Value}, null)%></li>
            <%
        }
        else
        {%>
            <li><%=Html.ActionLink("Cancel", "Details", "Topic", new { Id = Model.TopicId }, null)%></li>
            <%
        }%>
            <li><input type="submit" value="Preview" /></li>
            <li><input type="submit" value="Save" /></li>
        </ul>
    </fieldset>  
<%
    }%>
