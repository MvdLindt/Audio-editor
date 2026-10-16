# Accessibility notes and testing

This project is built with accessibility in mind. The initial app includes:

- AutomationProperties.Name and AutomationProperties.HelpText set on interactive controls.
- Clear status TextBlock that updates with actions (Open, Play, Stop, Trim) so screen readers announce changes.
- Keyboard shortcuts for the main actions (Alt+O Open, Space Play/Pause, Alt+S Stop, Alt+T Trim/Export)
- Logical Tab order.

Testing guidance
- Use Narrator (Windows) or NVDA for accessibility testing.
- Make sure "announce changes to the status" are read. If not, try focusing the status element after updates.

Further improvements
- Add UI Automation events for live regions if necessary.
- Add automated accessibility UI tests (e.g., using FlaUI or WinAppDriver).
