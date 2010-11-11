<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Weblitz.MessageBoard.Web.Models.DeleteItem>" %>

<%
    using (Html.BeginForm())
    {%>
    <fieldset>
        <p>Are you sure you want to delete <em><%:Model.Description%></em>?</p>
        <ul class="options">
            <li><%=Html.ActionLink("Cancel",
                                          Model.CancelNavigation.ActionName,
                                          Model.CancelNavigation.ControllerName,
                                          Model.CancelNavigation.RouteValues,
                                          null)%></li>
            <li><input type="submit" value="Delete" /></li>
        </ul>
    </fieldset>  
<%
    }%>
