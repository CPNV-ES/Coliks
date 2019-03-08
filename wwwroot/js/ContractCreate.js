/**
 * File for Contracts form
 */

document.getElementById('LastNames').onchange = async () => {
    try {
        const response = await fetch(`https://localhost:5001/api/names-list/${document.getElementById('LastNames').value}`, { 
            headers: { "Content-Type": "application/json" }
          })
        const data = await response.json()
        console.log(data)
    } catch (error) {
        console.log(error)
    }
}