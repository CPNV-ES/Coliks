/**
 * File for Contracts form
 */

document.getElementById('LastNames').onchange = async () => {
    try {
        const response = await fetch(`https://localhost:5001/api/names-list/${document.getElementById('LastNames').value}`, { 
            headers: { "Content-Type": "application/json" }
          })
        const customers = await response.json()
        if (customers.length > 0) {
            for (let i = document.getElementById('FirstName').length - 1; i >= 0; i--) {
                document.getElementById('FirstName').remove(i)
            }

            if (customers.length === 1) {
                let optionFirstName = document.createElement('option')
                optionFirstName.text = `${customers[0].firstname} (${customers[0].phone})`
                optionFirstName.value = customers[0].id
                document.getElementById('FirstName').add(optionFirstName)
                document.getElementById('Phone').value = customers[0].phone
                document.getElementById('Address').value = customers[0].address !== null ? customers[0].address : 'Non définie'
            } else {
                for (const customer of customers) {
                    let optionTemp = document.createElement('option')
                    optionTemp.text = `${customer.firstname} (${customer.phone})`
                    optionTemp.value = customer.id
                    document.getElementById('FirstName').add(optionTemp)
                    document.getElementById('FirstName').disabled = false
                }
            }
        }
    } catch (error) {
        console.log(error)
    }
}