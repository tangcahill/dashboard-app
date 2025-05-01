// wwwroot/alarm.js
window.launchAlarmIntent = (intentUrl) => {
    // top-level navigation is required for intent:// to work
    window.location.href = intentUrl;
};