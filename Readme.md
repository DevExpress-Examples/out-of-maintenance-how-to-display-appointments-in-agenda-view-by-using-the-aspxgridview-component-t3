# How to display appointments in Agenda View by using the ASPxGridView component 


<p>In fact, the Agenda view is a list of upcoming events grouped by an appointment's date. This list can be displayed in <a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxGridViewtopic">ASPxGridView</a>. </p>
<p>This example demonstrates how to implement this behavior.</p>
<p> </p>
<p>For demonstration purposes, we also show resources to which appointments belong. The resources are located in <a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxCardViewtopic">ASPxCardView</a>. Since multi-day appointments should be displayed as several ASPxGridView rows (such appointments should be displayed in each "Day" group in accordance with the appointment's duration), we use a separate AgendaAppointment class to store the appointment's data.<br /><br /></p>
<p>To get existing appointments from the ASPxScheduler's scheduler storage and generate a corresponding collection of AgendaAppointment instances, the <a href="https://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerSchedulerStorageBase_GetAppointmentstopic1830"><u>SchedulerStorage.GetAppointments</u></a> method is used.</p>
<p>By default, a month interval is used to fetch appointments for the AgendaView.<br /><br /></p>
<p><strong>Please see the "Implementation Details" (click the corresponding link below this text) to learn more about technical aspects of this approach implementation.</strong></p>


<h3>Description</h3>

<p>Both an&nbsp;ASPxScheduler and an&nbsp;&nbsp;<a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxGridViewtopic">ASPxGridView&nbsp;&nbsp;</a>instance (used as an AgendaView) are located in the same&nbsp;<a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxCallbackPaneltopic">ASPxCallbackPanel</a>. This allows updating&nbsp;the&nbsp;controls on a single callback.&nbsp;The ASPxGridView&nbsp;is located&nbsp;in&nbsp;the "<strong>AgendaViewControl</strong>" System.Web.UI.UserControl with an&nbsp;<a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxCardViewtopic">ASPxCardView&nbsp;</a>that serves to show resources.<br>Switching between the&nbsp;ASPxScheduler's&nbsp;view and an&nbsp;<a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxGridViewtopic">ASPxGridView</a>&nbsp;is performed by clicking a custom&nbsp;"AgendaView"&nbsp;menu item. This menu item click is processed on the client side&nbsp;(see the&nbsp;<strong>DefaultViewMenuHandler&nbsp;</strong>method implementation). To switch the active View to the AgendaView, the&nbsp;ASPxScheduler is hidden and a callback is sent to the&nbsp;&nbsp;<a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxCallbackPaneltopic">ASPxCallbackPanel</a>&nbsp;to update the visible interval in the AgendaView.&nbsp;&nbsp;</p>
<p>To generate a list of&nbsp;<strong>AgendaAppointment</strong>&nbsp;instances depending on existing appointments, the&nbsp;<strong>AgendaViewDataGenerator</strong>&nbsp;is used.</p>
<p>A context menu for the AgendaView is built with the static&nbsp;<strong>ContextMenuHelper&nbsp;</strong>class.</p>

<br/>


