

const apiPath = "http://localhost:5000"

async function GetAverageExchangeRate(code: string, date: Date) {
    let path = `${apiPath}/exchange/average/${code}/${date.getFullYear()}-${date.getMonth()}-${date.getDay()}`;
    const response = await fetch(path);
    const data = await response.json();
    console.log(data);
}

export {GetAverageExchangeRate};