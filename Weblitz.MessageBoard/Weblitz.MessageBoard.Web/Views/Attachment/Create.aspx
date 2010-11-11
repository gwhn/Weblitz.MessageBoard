<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Weblitz.MessageBoard.Core.Domain.Model.Attachment>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Attachment
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    using (Html.BeginForm("Create", "Attachment", new {EntryId = Model.Entry.Id}, FormMethod.Post, new {enctype = "multipart/form-data"}))
    {%>
    <fieldset>
        <div class="editor-label">
            File
        </div>
        <div class="editor-field">
            <input type="file" name="file" />
        </div>
        <ul class="options">
            <li><%=Html.ActionLink("Cancel", "Index", "Forum")%></li>
            <li><input type="submit" value="Upload" /></li>
        </ul>
    </fieldset>
<%
    }%>
</asp:Content>
