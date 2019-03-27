/**
 * File for Contracts form
 */
let customers = []
let contracts = []
let durations = []
let items = []
let contract = {
    customerId: null,
    notes: "",
    total: null,
    takenon: false,
    paidon: false,
    insurance: false,
    goget: false,
    helpStaffId: document.getElementById('HelpStaffId').value,
    tuneStaffId: document.getElementById('TuneStaffId').value,
    rentedItems: []
}

// Add event listener on click to enable address and locality
document.getElementById('address-lock').onclick = () => {
    document.getElementById('Address').disabled = false
    document.getElementById('Locality').disabled = false
    document.getElementById('address-lock').style.display = 'none'
    document.getElementById('address-unlock').style.display = 'block'
}

// Add event listener on click to disable address and locality
document.getElementById('address-unlock').onclick = () => {
    document.getElementById('Address').disabled = true
    document.getElementById('Locality').disabled = true
    document.getElementById('address-unlock').style.display = 'none'
    document.getElementById('address-lock').style.display = 'block'
}

// Event listener on Help staff dropdown, add the value to the contract object and changes the Tune staff value
document.getElementById('HelpStaffId').onchange = () => {
    contract.helpStaffId = Number(document.getElementById('HelpStaffId').value)
    contract.tuneStaffId = Number(document.getElementById('HelpStaffId').value)
    document.getElementById('TuneStaffId').value = document.getElementById('HelpStaffId').value
}

// Update the contract object with new value in Tune staff dropdown
document.getElementById('TuneStaffId').onchange = () => {
    contract.tuneStaffId = Number(document.getElementById('TuneStaffId').value)
}

// Update contract object with new value in PaidOn checkbox
document.getElementById('PaidOn').onchange = () => {
    contract.paidon = document.getElementById('PaidOn').checked
}

// Update contract object with new value in TakenOn checkbox
document.getElementById('TakenOn').onchange = () => {
    contract.takenon = document.getElementById('TakenOn').checked
}

// Update contract object with new value in Insurance checkbox
document.getElementById('Insurance').onchange = () => {
    contract.insurance = document.getElementById('Insurance').checked
}

// Update contract object with new value in GoGet checkbox
document.getElementById('GoGet').onchange = () => {
    contract.goget = document.getElementById('GoGet').checked
}

// Update contract object with new value in Notes textarea
document.getElementById('Notes').oninput = (e) => {
    contract.notes = e.target.value
}

// Get all the contracts of a customer
getCustomerContracts = async (id) => {
    try {
        const response = await fetch(`https://localhost:5001/api/customer-contracts/${id}`)
        const contracts = await response.json()

        return contracts
    } catch (error) {
        showAlertMessage(error)
    }
}

// Function to show an alert message with an error as a parameter
showAlertMessage = (error) => {
    const alert = document.getElementById('AlertMessage')
    alert.innerHTML = error
    $('#AlertMessage').fadeIn()
    setTimeout(() => { $('#AlertMessage').fadeOut() }, 1500)
}

// Function to add an item slot. Get the table body from parameter
addItemSlot = (tableBody) => {
    // Create a table row on the table body
    tableBody.appendChild(document.createElement('tr'))
    // Gett the row created before
    const row = tableBody.getElementsByTagName('tr')[tableBody.childElementCount - 1]
    // Create the column for the brand/model input
    const rowItems = document.createElement('td')
    // Create the column for the item number input
    const rowItemNumber = document.createElement('td')
    // Create the input with the Bootstrap class and link the datalist
    const inputNumber = document.createElement('input')
    inputNumber.setAttribute('list', `itemnb${tableBody.childElementCount - 1}`)
    inputNumber.classList.add('form-control')
    // Create the datalist that will hold the item numbers
    const datalistItemNb = document.createElement('datalist')
    datalistItemNb.id = `itemnb${tableBody.childElementCount - 1}`
    // Create the input for the brand/model
    const selectItems = document.createElement('input')
    selectItems.setAttribute('list', `items${tableBody.childElementCount - 1}`)
    selectItems.classList.add('form-control')
    // Create the datalist that will hold the brand/model items
    const datalistItems = document.createElement('datalist')
    datalistItems.id = `items${tableBody.childElementCount - 1}`
    // Create the category row and input
    const rowCategory = document.createElement('td')
    const inputCategory = document.createElement('input')
    inputCategory.classList.add('form-control')
    inputCategory.type = 'number'
    rowCategory.appendChild(inputCategory)
    // Create the row for the price
    const rowPrice = document.createElement('td')

    // Function that will be called by inputs for the datalist
    onInput = async (datalist, event, type) => {
        // Remove all the children of the datalist
        while (datalist.hasChildNodes()) {
            datalist.removeChild(datalist.lastChild)
        }

        // Check if items were found
        if (items.length > 0) {
            // Check if the user selected from the datalist, since there's no event for it
            const inputFromDatalist = items.filter(item => {
                return event.target.value == parseInt(item.id)
            })

            // If it's the case
            if (inputFromDatalist.length > 0) {
                // Get the index of the item to check if it's an update
                const itemIndex = contract.rentedItems.findIndex(selectedItem => {
                    return selectedItem.id == event.target.value
                })

                // Create the item object that will be put in the contract object
                const item = {
                    categoryId: inputFromDatalist[0].category.id,
                    itemId: inputFromDatalist[0].id,
                    durationId: Number(selectDuration.value),
                    itemnb: inputFromDatalist[0].itemnb,
                    type: inputFromDatalist[0].type
                }
                // Get the price of the item depending on the type, the category and the duration
                const responsePrice = await fetch(`https://localhost:5001/api/get-price?CategoryId=${item.categoryId}&ItemType=${inputFromDatalist[0].type}&DurationId=${item.durationId}`)
                const itemPrice = await responsePrice.json()
                item['price'] = itemPrice.price
    
                // Update item if already in array
                if (itemIndex >= 0) {
                    contract.rentedItems[itemIndex] = item
                } else {
                    contract.rentedItems.push(item)
                }

                document.getElementById('SubmitContract').disabled = false
                inputNumber.classList.remove('is-invalid')
                selectItems.classList.remove('is-invalid')
                // Fill the other input with the information from API
                inputCategory.value = inputFromDatalist[0].category.code
                inputNumber.value = inputFromDatalist[0].itemnb
                selectItems.value = `${inputFromDatalist[0].brand} : ${inputFromDatalist[0].model}`
                rowPrice.innerText = itemPrice.price
            }
        } else {
            document.getElementById('SubmitContract').disabled = true
            inputNumber.classList.add('is-invalid')
            selectItems.classList.add('is-invalid')
        }

        // Fetch the list of items that match the value inputted after the user typed twice
        if (event.target.value.length >= 2) {
            const responseItems = await fetch(`https://localhost:5001/api/items?${type == 'itemNumber' ? 'inputNumber' : 'input'}=${event.target.value}`)
            items = await responseItems.json()

            for (const item of items) {
                const option = document.createElement('option')
                option.value = item.id
                option.text = type == 'itemNumber' ? item.itemnb : `${item.brand} : ${item.model}`
                datalist.appendChild(option)
            }
        }
    }

    // Call the function onInput with the parameters for the item number
    inputNumber.oninput = async (e) => {
        onInput(datalistItemNb, e, 'itemNumber')
    }

    // Call the function onInput with the parameters for the item number
    selectItems.oninput = async (e) => {
        onInput(datalistItems, e, 'itemName')
    }

    // Add event listener on category input
    inputCategory.oninput = async (e) => {
        // Add invalid to input if it's empty
        if (e.target.value == "") {
            inputCategory.classList.add('is-invalid')
            document.getElementById('SubmitContract').disabled = true
        }
        // If the category code is between 0 and 3 and not empty
        if (e.target.value >= 0 && e.target.value <= 3 && e.target.value != "") {
            // Get the current item and its index
            const currentItem = contract.rentedItems.filter(item => {
                return item.itemnb === inputNumber.value
            })
            const currentItemIndex = contract.rentedItems.findIndex(item => {
                return item.itemnb === inputNumber.value
            })

            // If it's found
            if (currentItem.length > 0 && currentItemIndex >= 0) {
                // Fetch the price with the new category
                await fetch(`https://localhost:5001/api/change-item-category/${currentItem[0].itemId}`, {
                    method: 'PUT',
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ code: Number(e.target.value) })
                })
                const responsePrice = await fetch(`https://localhost:5001/api/get-price?ItemType=${currentItem[0].type}&DurationId=${currentItem[0].durationId}&CategoryCode=${e.target.value}`)
                const newPrice = await responsePrice.json()

                document.getElementById('SubmitContract').disabled = false
                inputCategory.classList.remove('is-invalid')
                contract.rentedItems[currentItemIndex].categoryId = newPrice.category.id
                contract.rentedItems[currentItemIndex].price = newPrice.price
                rowPrice.innerText = newPrice.price
            }
        }
    }

    // Append all the children to create the row
    rowItemNumber.appendChild(inputNumber)
    rowItemNumber.appendChild(datalistItemNb)
    row.appendChild(rowItemNumber)
    rowItems.appendChild(selectItems)
    rowItems.appendChild(datalistItems)
    row.appendChild(rowItems)
    row.appendChild(rowCategory)
    // Create the column for duration
    const rowDuration = document.createElement('td')
    // Create the select element for duration
    const selectDuration = document.createElement('select')
    selectDuration.classList.add('form-control')

    // Create the options from the durations from API
    for (let duration of durations) {
        const option = document.createElement('option')
        option.text = duration.details
        option.value = duration.id
        selectDuration.add(option)
    }

    // Add event listener on change of the select
    selectDuration.onchange = async (e) => {
        // Get the current item and its index
        const currentItem = contract.rentedItems.filter(item => {
            return item.itemnb === inputNumber.value
        })
        const currentItemIndex = contract.rentedItems.findIndex(item => {
            return item.itemnb === inputNumber.value
        })

        // If the item is found
        if (currentItem.length > 0 && currentItemIndex >= 0) {
            // Get the new price with the new duration
            const response = await fetch(`https://localhost:5001/api/get-price?ItemType=${currentItem[0].type}&DurationId=${e.target.value}&CategoryId=${currentItem[0].categoryId}`)
            const newPrice = await response.json()

            contract.rentedItems[currentItemIndex].durationId = e.target.value
            contract.rentedItems[currentItemIndex].price = newPrice.price
            rowPrice.innerText = newPrice.price
        }
    }

    // Append all the columns to the new row
    rowDuration.appendChild(selectDuration)
    row.appendChild(rowDuration)
    row.appendChild(rowPrice)
    const rowDelete = document.createElement('td')
    const buttonDelete = document.createElement('button')
    buttonDelete.classList.add('btn', 'btn-danger', 'btn-sm')
    buttonDelete.innerText = 'Supprimer'

    // Add event listener on click of the delete item button
    buttonDelete.onclick = () => {
        const deletedItemIndex = contract.rentedItems.findIndex(item => {
            return item.itemnb === inputNumber.value
        })
        contract.rentedItems.splice(deletedItemIndex, 1)
        tableBody.removeChild(row)
    }
    
    rowDelete.appendChild(buttonDelete)
    row.appendChild(rowDelete)
}

// Remove the contracts table and display the items table
fillItemsTable = () => {
    document.getElementById('CustomerContracts').style.display = 'none'
    const tableBody = document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0]
    addItemSlot(tableBody)
    document.getElementById('ItemsContracts').style.display = 'block'
}

// Function to remove an item from the table
removeItemsTable = () => {
    document.getElementById('NewContract').style.display = 'block'
    document.getElementById('ItemsContracts').style.display = 'none'
    document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0].parentNode.replaceChild(
        document.createElement('tbody'),
        document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0]
    )
}

// Function to fill the contracts table with the contracts of the selected customer
fillContractsTable = () => {
    // Get the old tbody to replace it with the new one
    const oldTableBody = document.getElementById('CustomerContracts').getElementsByTagName('tbody')[0]
    const newTableBody = document.createElement('tbody')
    // Create all the columns with the id, the total and a link to the contract
    for (const [index, contract] of contracts.entries()) {
        newTableBody.appendChild(document.createElement('tr'))
        const row = newTableBody.getElementsByTagName('tr')[index]
        const rowId = document.createElement('th')
        rowId.setAttribute('scope', 'row')
        rowId.innerText = contract.id
        row.appendChild(rowId)
        const rowTotal = document.createElement('td')
        rowTotal.innerText = contract.total
        row.appendChild(rowTotal)
        const rowButton = document.createElement('td')
        const detailsButton = document.createElement('a')
        detailsButton.setAttribute('role', 'button')
        detailsButton.classList.add('btn', 'btn-info')
        detailsButton.innerText = 'Consulter'
        detailsButton.setAttribute('href', `/Contracts/Details/${contract.id}`)
        rowButton.appendChild(detailsButton)
        row.appendChild(rowButton)
    }
    oldTableBody.parentNode.replaceChild(newTableBody, oldTableBody)
    document.getElementById('CustomerContracts').style.display = 'block'
}

// Function to fill the different inputs when a customer is selected
setCustomerInfo = async (customer = customers[0]) => {
    try {
        contract.customerId = customer.id
        document.getElementById('Email').value = customer.email !== null ? customer.email : 'Non défini'
        document.getElementById('Mobile').value = customer.mobile !== null ? customer.mobile : 'Non défini'
        document.getElementById('Locality').value = customer.city !== null ? customer.city.name : 'Non définie'
        document.getElementById('Phone').value = customer.phone !== null ? customer.phone : 'Non défini'
        document.getElementById('Address').value = customer.address !== null ? customer.address : 'Non définie'
        contracts = await getCustomerContracts(customer.id)
        document.getElementById('NewContract').disabled = false

        fillContractsTable()
    } catch (error) {
        showAlertMessage(error)
    }
}

// Function to set the customer information when a last name is chosen
document.getElementById('LastNames').onchange = async () => {
    try {
        // Remove the items table if it's created
        removeItemsTable()
        document.getElementById('LastNames').classList.remove('is-invalid')
        const response = await fetch(`https://localhost:5001/api/names-list/${document.getElementById('LastNames').value}`, { 
            headers: { "Content-Type": "application/json" }
          })
        customers = await response.json()
        if (customers.length > 0) {
            // Remove all the old first names in the select
            for (let i = document.getElementById('FirstName').length - 1; i >= 0; i--) {
                document.getElementById('FirstName').remove(i)
            }

            // If there's only 1 customer for this last name, only create an option with his/her name
            if (customers.length === 1) {
                document.getElementById('FirstName').disabled = true
                let optionFirstName = document.createElement('option')
                optionFirstName.text = `${customers[0].firstname} (${customers[0].phone !== null ? customers[0].phone : customers[0].mobile !== null ? customers[0].mobile : 'Non défini'})`
                optionFirstName.value = customers[0].id
                document.getElementById('FirstName').add(optionFirstName)
                setCustomerInfo()
            // Else, create all the options with the first names
            } else {
                for (const customer of customers) {
                    let optionTemp = document.createElement('option')
                    optionTemp.text = `${customer.firstname} (${customer.phone !== null ? customer.phone : customer.mobile !== null ? customer.mobile : 'Non défini'})`
                    optionTemp.value = customer.id
                    document.getElementById('FirstName').add(optionTemp)
                    document.getElementById('FirstName').disabled = false
                }
                setCustomerInfo()
            }
        }
    } catch (error) {
        showAlertMessage(error)
    }
}

// Set the selected customer when first name changes
document.getElementById('FirstName').onchange = async () => {
    try {
        let selectedCustomer = null
        for (const customer of customers) {
            if (customer.id === Number(document.getElementById('FirstName').value)) selectedCustomer = customer
        }

        setCustomerInfo(selectedCustomer)
        fillContractsTable()
    } catch (error) {
        showAlertMessage(error)
    }
}

// Function to create and display the items table
document.getElementById('NewContract').onclick = async (e) => {
    try {
        e.preventDefault()
        document.getElementById('SubmitContract').disabled = true
        const responseDurations = await fetch('https://localhost:5001/api/durations')
        durations = await responseDurations.json()

        document.getElementById('NewContract').style.display = 'none'
        fillItemsTable()
    } catch (error) {
        showAlertMessage(error)
    }
}

// Disable the create button when an item slot is added
document.getElementById('AddItem').onclick = () => {
    document.getElementById('SubmitContract').disabled = true
    addItemSlot(document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0])
}

// Function to validate if the contract can be created
validateForm = () => {
    // Check if a last name was chosen
    if (document.getElementById('LastNames').value !== "") {
        if (contract.rentedItems.length > 0) {
            return true
        } else {
            showAlertMessage('Pas d\'objets sélectionnés')
            return false
        }
    } else {
        document.getElementById('LastNames').classList.add('is-invalid')
        showAlertMessage('Pas de client sélectionné')
        return false
    }
}

// Add event listener on click of the create contract button
document.getElementById('SubmitContract').onclick = async (e) => {
    e.preventDefault()
    // Check if the form is valid
    if (validateForm()) {
        // Calculate the total of all the items in the contract
        let total = 0
        for (const item of contract.rentedItems) {
            total += item.price
            // Delete 2 useless information for the submit in the item object
            delete item.itemnb
            delete item.type
        }
        contract.total = total

        // Prepare the object for the API
        contract.takenon === true ? contract.takenon = null : contract.takenon = new Date()
        contract.paidon === true ? contract.paidon = null : contract.paidon = new Date()
        contract.goget === true ? contract.goget = 1 : contract.goget = 0
        contract.insurance === true ? contract.insurance = 1 : contract.insurance = 0

        const response = await fetch('https://localhost:5001/api/contracts/create', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(contract)
        })
        const newContract = await response.json()
    }
}