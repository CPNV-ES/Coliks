/**
 * File for Contracts form
 */

let customers = []
let contracts = []
let items = []
let durations = []

getCustomerContracts = async (id) => {
    try {
    const response = await fetch(`https://localhost:5001/api/customer-contracts/${id}`)
    const contracts = await response.json()

    return contracts
    } catch (error) {
        showAlertMessage(error)
    }
}

showAlertMessage = (error) => {
    const alert = document.getElementById('AlertMessage')
    alert.innerHTML = error
    $('#AlertMessage').fadeIn()
    setTimeout(() => { $('#AlertMessage').fadeOut() }, 1500)
}

addItemSlot = (tableBody) => {
    tableBody.appendChild(document.createElement('tr'))
    const row = tableBody.getElementsByTagName('tr')[tableBody.childElementCount - 1]
    const rowItems = document.createElement('td')
    const selectItems = document.createElement('select')
    selectItems.classList.add('form-control')

    for (let item of items) {
        const option = document.createElement('option')
        option.text = `${item.brand} : ${item.model}`
        option.value = `${item.id}`
        selectItems.add(option)
    }

    selectItems.onchange = () => {
        const item = items.filter(i => {
            return i.id == selectItems.value
        })
        selectItems.parentNode.parentNode.childNodes[1].innerText = item[0].stock
    }

    rowItems.appendChild(selectItems)
    row.appendChild(rowItems)
    const rowStock = document.createElement('td')
    rowStock.innerText = items[0].stock
    row.appendChild(rowStock)
    const rowDuration = document.createElement('td')
    const selectDuration = document.createElement('select')
    selectDuration.classList.add('form-control')

    for (let duration of durations) {
        const option = document.createElement('option')
        option.text = duration.details
        option.value = duration.id
        selectDuration.add(option)
    }

    rowDuration.appendChild(selectDuration)
    row.appendChild(rowDuration)
    const rowDelete = document.createElement('td')
    const buttonDelete = document.createElement('button')
    buttonDelete.classList.add('btn', 'btn-danger', 'btn-sm')
    buttonDelete.innerText = 'Supprimer'

    buttonDelete.onclick = () => {
        tableBody.removeChild(row)
    }
    
    rowDelete.appendChild(buttonDelete)
    row.appendChild(rowDelete)
}

fillItemsTable = () => {
    document.getElementById('CustomerContracts').style.display = 'none'
    const tableBody = document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0]
    addItemSlot(tableBody)
    document.getElementById('ItemsContracts').style.display = 'block'
}

fillContractsTable = () => {
    const oldTableBody = document.getElementById('CustomerContracts').getElementsByTagName('tbody')[0]
    const newTableBody = document.createElement('tbody')
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

setCustomerInfo = async (customer = customers[0]) => {
    try {
        document.getElementById('Email').value = customer.email !== null ? customer.email : 'Non défini'
        document.getElementById('Mobile').value = customer.mobile !== null ? customer.mobile : 'Non défini'
        document.getElementById('Locality').value = customer.city !== null ? customer.city : 'Non définie'
        document.getElementById('Phone').value = customer.phone !== null ? customer.phone : 'Non défini'
        document.getElementById('Address').value = customer.address !== null ? customer.address : 'Non définie'
        contracts = await getCustomerContracts(customer.id)
        document.getElementById('NewContract').disabled = false

        fillContractsTable()
    } catch (error) {
        showAlertMessage(error)
    }
}

document.getElementById('LastNames').onchange = async () => {
    try {
        const response = await fetch(`https://localhost:5001/api/names-list/${document.getElementById('LastNames').value}`, { 
            headers: { "Content-Type": "application/json" }
          })
        customers = await response.json()
        if (customers.length > 0) {
            for (let i = document.getElementById('FirstName').length - 1; i >= 0; i--) {
                document.getElementById('FirstName').remove(i)
            }

            document.getElementById('Phone').value = null
            document.getElementById('Address').value = null

            if (customers.length === 1) {
                document.getElementById('FirstName').disabled = true
                let optionFirstName = document.createElement('option')
                optionFirstName.text = `${customers[0].firstname} (${customers[0].phone !== null ? customers[0].phone : customers[0].mobile !== null ? customers[0].mobile : 'Non défini'})`
                optionFirstName.value = customers[0].id
                document.getElementById('FirstName').add(optionFirstName)
                setCustomerInfo()
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

document.getElementById('NewContract').onclick = async (e) => {
    try {
        e.preventDefault()
        const responseItems = await fetch('https://localhost:5001/api/items')
        items = await responseItems.json()

        const responseDurations = await fetch('https://localhost:5001/api/durations')
        durations = await responseDurations.json()

        document.getElementById('NewContract').style.display = 'none'
        fillItemsTable()
    } catch (error) {
        showAlertMessage(error)
    }
}

document.getElementById('AddItem').onclick = () => {
    addItemSlot(document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0])
}