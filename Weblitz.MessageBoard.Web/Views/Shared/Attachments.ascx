<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Weblitz.MessageBoard.Core.Domain.Model.Attachment[]>" %>

<div class="attachmentslist">
    <h3>Attachments</h3>
    <%
        if (Model.Length > 0)
        {%>
    <table>
    <%
            foreach (var attachment in Model)
            {%>
        <tr>
            <td><%=Html.ActionLink(attachment.FileName, "Download", "Attachment",
                                                  new {attachment.Id}, null)%></td>
            <td><%=Html.ActionLink("Delete", "Delete", "Attachment", new {attachment.Id}, null)%></td>
        </tr>
        <%
            }%>
    </table>
    <%
        }
        else
        {%>
        <div class="emptylist">There are no attachments</div>
    <%
        }%>
</div>
