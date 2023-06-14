document.addEventListener('DOMContentLoaded', function() {
        var adminPanelTrigger = document.querySelector('.admin-panel-trigger');
        var adminPanel = document.querySelector('.admin-panel');

        adminPanelTrigger.addEventListener('click', function() {
            adminPanel.classList.toggle('show');
		});
});