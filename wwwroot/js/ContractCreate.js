/**
 * File for Contracts form
 */

let customers = []
let contracts = []
let durations = []
let items = []
let selectedItems = []

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
    const rowItemNumber = document.createElement('td')
    const inputNumber = document.createElement('input')
    inputNumber.setAttribute('list', `itemnb${tableBody.childElementCount - 1}`)
    inputNumber.classList.add('form-control')
    const datalistItemNb = document.createElement('datalist')
    datalistItemNb.id = `itemnb${tableBody.childElementCount - 1}`
    const selectItems = document.createElement('input')
    selectItems.setAttribute('list', `items${tableBody.childElementCount - 1}`)
    selectItems.classList.add('form-control')
    const datalistItems = document.createElement('datalist')
    datalistItems.id = `items${tableBody.childElementCount - 1}`
    const rowCategory = document.createElement('td')
    const inputCategory = document.createElement('input')
    inputCategory.classList.add('form-control')
    inputCategory.type = 'number'
    rowCategory.appendChild(inputCategory)
    const rowPrice = document.createElement('td')

    onInput = async (datalist, event, type) => {
        while (datalist.hasChildNodes()) {
            datalist.removeChild(datalist.lastChild)
        }

        if (items.length > 0) {
            const inputFromDatalist = items.filter(item => {
                return event.target.value == parseInt(item.id)
            })

            if (inputFromDatalist.length > 0) {
                const itemIndex = selectedItems.findIndex(selectedItem => {
                    return selectedItem.id == event.target.value
                })

                const item = {
                    item: inputFromDatalist[0],
                    durationId: Number(selectDuration.value)
                }
    
                if (itemIndex >= 0) {
                    selectedItems[itemIndex] = item
                } else {
                    selectedItems.push(item)
                }

                const responsePrice = await fetch(`https://localhost:5001/api/get-price?CategoryId=${item.item.category.id}&ItemType=${item.item.type}&DurationId=${item.durationId}`)
                const itemPrice = await responsePrice.json()

                inputCategory.value = inputFromDatalist[0].category.code
                inputNumber.value = inputFromDatalist[0].itemnb
                selectItems.value = `${inputFromDatalist[0].brand} : ${inputFromDatalist[0].model}`
                rowPrice.innerText = itemPrice.price
            }
        }

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

    inputNumber.oninput = async (e) => {
        onInput(datalistItemNb, e, 'itemNumber')
    }

    selectItems.oninput = async (e) => {
        onInput(datalistItems, e, 'itemName')
    }

    getCurrentItem = () => {
        return selectedItems.filter(item => {
            return item.item.itemnb === inputNumber.value
        })
    }

    getCurrentItemIndex = () => {
        return selectedItems.findIndex(item => {
            return item.item.itemnb === inputNumber.value
        })
    }

    inputCategory.oninput = async (e) => {
        if (e.target.value >= 0 && e.target.value <= 3 && e.target.value != "") {
            const currentItem = getCurrentItem()
            const currentItemIndex = getCurrentItemIndex()

            if (currentItem.length > 0 && currentItemIndex >= 0) {
                const response = await fetch(`https://localhost:5001/api/get-price?ItemType=${currentItem[0].item.type}&DurationId=${currentItem[0].durationId}&CategoryCode=${e.target.value}`)
                const newPrice = await response.json()

                selectedItems[currentItemIndex].category = newPrice.category
                rowPrice.innerText = newPrice.price
            }
        }
    }

    rowItemNumber.appendChild(inputNumber)
    rowItemNumber.appendChild(datalistItemNb)
    row.appendChild(rowItemNumber)
    rowItems.appendChild(selectItems)
    rowItems.appendChild(datalistItems)
    row.appendChild(rowItems)
    row.appendChild(rowCategory)
    const rowDuration = document.createElement('td')
    const selectDuration = document.createElement('select')
    selectDuration.classList.add('form-control')

    for (let duration of durations) {
        const option = document.createElement('option')
        option.text = duration.details
        option.value = duration.id
        selectDuration.add(option)
    }

    selectDuration.onchange = async (e) => {
        const currentItem = getCurrentItem()
        const currentItemIndex = getCurrentItemIndex()

        if (currentItem.length > 0 && currentItemIndex >= 0) {
            const response = await fetch(`https://localhost:5001/api/get-price?ItemType=${currentItem[0].item.type}&DurationId=${e.target.value}&CategoryId=${currentItem[0].item.category.id}`)
            const newPrice = await response.json()

            selectedItems[currentItemIndex].durationId = e.target.value
            rowPrice.innerText = newPrice.price
        }
    }

    rowDuration.appendChild(selectDuration)
    row.appendChild(rowDuration)
    row.appendChild(rowPrice)
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

removeItemsTable = () => {
    document.getElementById('NewContract').style.display = 'block'
    document.getElementById('ItemsContracts').style.display = 'none'
    document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0].parentNode.replaceChild(
        document.createElement('tbody'),
        document.getElementById('ItemsContractsTable').getElementsByTagName('tbody')[0]
    )
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

document.getElementById('LastNames').onchange = async () => {
    try {
        removeItemsTable()
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