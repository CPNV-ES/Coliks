/**
 * File for Contracts form
 */

let customers = []
let contracts = []
let items = []

getCustomerContracts = async (id) => {
    const response = await fetch(`https://localhost:5001/api/customer-contracts/${id}`)
    const contracts = await response.json()

    return contracts
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
    rowDuration.appendChild(selectDuration)
    row.appendChild(rowDuration)
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
        detailsButton.classList.add('btn')
        detailsButton.classList.add('btn-info')
        detailsButton.innerText = 'Consulter'
        detailsButton.setAttribute('href', `/Contracts/Details/${contract.id}`)
        rowButton.appendChild(detailsButton)
        row.appendChild(rowButton)
    }
    oldTableBody.parentNode.replaceChild(newTableBody, oldTableBody)
    document.getElementById('CustomerContracts').style.display = 'block'
}

setCustomerInfo = async () => {
    document.getElementById('Phone').value = customers[0].phone !== null ? customers[0].phone : 'Non défini'
    document.getElementById('Address').value = customers[0].address !== null ? customers[0].address : 'Non définie'
    contracts = await getCustomerContracts(customers[0].id)
    document.getElementById('NewContract').disabled = false

    fillContractsTable()
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
                optionFirstName.text = `${customers[0].firstname} (${customers[0].phone})`
                optionFirstName.value = customers[0].id
                document.getElementById('FirstName').add(optionFirstName)
                setCustomerInfo()
            } else {
                for (const customer of customers) {
                    let optionTemp = document.createElement('option')
                    optionTemp.text = `${customer.firstname} (${customer.phone})`
                    optionTemp.value = customer.id
                    document.getElementById('FirstName').add(optionTemp)
                    document.getElementById('FirstName').disabled = false
                }
                setCustomerInfo()
            }
        }
    } catch (error) {
        console.log(error)
    }
}

document.getElementById('FirstName').onchange = async () => {
    let selectedCustomer = null
    for (const customer of customers) {
        if (customer.id === Number(document.getElementById('FirstName').value)) selectedCustomer = customer
    }

    document.getElementById('Phone').value = selectedCustomer.phone !== null ? selectedCustomer.phone : 'Non défini'
    document.getElementById('Address').value = selectedCustomer.address !== null ? selectedCustomer.address : 'Non définie'
    contracts = await getCustomerContracts(Number(document.getElementById('FirstName').value))
    document.getElementById('NewContract').disabled = false
    fillContractsTable()
}

document.getElementById('NewContract').onclick = async (e) => {
    e.preventDefault()
    const response = await fetch('https://localhost:5001/api/items')
    items = await response.json()

    document.getElementById('NewContract').style.display = 'none'
    fillItemsTable()
}

document.getElementById('AddItem').onclick = () => {
    addItemSlot(document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0])
}