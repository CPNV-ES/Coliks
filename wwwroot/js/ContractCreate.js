/**
 * File for Contracts form
 */

let customers = []
let contracts = []

getCustomerContracts = async (id) => {
    const response = await fetch(`https://localhost:5001/api/customer-contracts/${id}`)
    const contracts = await response.json()

    return contracts
}

setCustomerInfo = async () => {
    document.getElementById('Phone').value = customers[0].phone !== null ? customers[0].phone : 'Non défini'
    document.getElementById('Address').value = customers[0].address !== null ? customers[0].address : 'Non définie'
    contracts = await getCustomerContracts(customers[0].id)
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
}