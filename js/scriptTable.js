function bookTable(tableNumber) {
    const timeInput = document.getElementById(`time-${tableNumber}`);
    const selectedTime = timeInput.value;

    if (!selectedTime) {
        alert('Please select a time for your reservation.');
        return;
    }

    // Simulate sending request to the manager
    const request = {
        table: tableNumber,
        time: selectedTime,
        date: new Date().toLocaleDateString()
    };

    // Here you would send the request to your server
    console.log('Sending request:', request);

    // Simulate manager response
    const managerResponse = confirm(`Manager: Confirm booking for Table ${tableNumber} at ${selectedTime}?`);

    if (managerResponse) {
        alert(`Booking confirmed for Table ${tableNumber} at ${selectedTime}.`);
    } else {
        alert(`Booking for Table ${tableNumber} at ${selectedTime} was rejected.`);
    }
}
