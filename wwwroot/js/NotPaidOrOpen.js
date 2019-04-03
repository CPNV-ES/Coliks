// Arrays that will contain the paid or returned contracts that were checked
let paid = []
let returned = []

searchCustomerByName = (event, className, row) => {
    if (event.target.value.length >= 2) {
        const name = row.getElementsByClassName(className)[0].innerHTML.split(' ')
        if (!name[0].toLowerCase().includes(event.target.value.toLowerCase()) &&
            !name[1].toLowerCase().includes(event.target.value.toLowerCase())
        ) {
            row.style.display = 'none'
        } else {
            row.style.display = 'table-row'
        }
    } else {
        row.style.display = 'table-row'
    }
}

document.getElementById('search-paid').oninput = (event) => {
    for (const row of document.getElementById('not-paid-tbody').children) {
        searchCustomerByName(event, 'customer-name-paid', row)
    }
}

document.getElementById('search-returned').oninput = (event) => {
    for (const row of document.getElementById('not-returned-tbody').children) {
        searchCustomerByName(event, 'customer-name-returned', row)
    }
}

// Function to show an alert message with an error as a parameter
showAlertMessage = (error) => {
    const alert = document.getElementById('AlertMessage')
    alert.innerHTML = error
    $('#AlertMessage').fadeIn()
    setTimeout(() => { $('#AlertMessage').fadeOut() }, 1500)
}

// Iterate over all checkboxes in the paid table
for (const checkboxNotPaid of document.getElementsByClassName('checkboxes-not-paid')) {
    // Add an event listener to add or remove values in paid array
    checkboxNotPaid.onchange = () => {
        if (checkboxNotPaid.checked) {
            // Push the contract id and the row that was selected to remove it later dynamically
            paid.push({
                contractId: checkboxNotPaid.parentElement.parentElement.getElementsByTagName('th')[0].innerText,
                row: checkboxNotPaid.parentElement.parentElement
            })
        } else {
            // Remove the contract if checkbox gets disabled
            paid = paid.filter(p => p.contractId != checkboxNotPaid.parentElement.parentElement.getElementsByTagName('th')[0].innerText)
        }
    }
}

// Iterate over all checkboxes in the returned table
for (const checkboxNotReturned of document.getElementsByClassName('checkboxes-not-returned')) {
    // Add an event listener to add or remove values in returned array
    checkboxNotReturned.onchange = () => {
        if (checkboxNotReturned.checked) {
            // Push the contract id and the row that was selected to remove it later dynamically
            returned.push({
                contractId: checkboxNotReturned.parentElement.parentElement.getElementsByTagName('th')[0].innerText,
                row: checkboxNotReturned.parentElement.parentElement
            })
        } else {
            // Remove the contract if checkbox gets disabled
            returned = returned.filter(r => r.contractId != checkboxNotReturned.parentElement.parentElement.getElementsByTagName('th')[0].innerText)
        }
    }
}

// Add event listener on click of the paid button
document.getElementById('paid').onclick = async () => {
    // Iterate over all contracts that were pushed into the paid array
    for (const contractId of paid) {
        try {
            // Contact API to modify the contract and say it was paid
            await fetch(`https://localhost:5001/api/paid-contract/${contractId.contractId}`, {
                method: 'PUT'
            })
            // Remove the contract from the table
            document.getElementById('not-paid-tbody').removeChild(contractId.row)
        } catch (error) {
            // Display an error message if an error occurs
            showAlertMessage('Une erreur est survenue')
            break
        }
    }
    // Reset the array so the user can check other contracts after clicking on the button
    paid = []
}

// Add event listener on click of the returned button
document.getElementById('returned').onclick = async () => {
    // Iterate over all contracts that were pushed into the returned array
    for (const contractId of returned) {
        try {
            // Contact API to modify the contract and say it was returned
            await fetch(`https://localhost:5001/api/returned-contract/${contractId.contractId}`, {
                method: 'PUT'
            })
            // Remove the contract from the table
            document.getElementById('not-returned-tbody').removeChild(contractId.row)
        } catch (error) {
            // Display an error message if an error occurs
            showAlertMessage('Une erreur est survenue')
            break
        }
    }
    // Reset the array so the user can check other contracts after clicking on the button
    returned = []
}

