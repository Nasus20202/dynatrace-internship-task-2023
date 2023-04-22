
const apiPath = "http://localhost:5000"

interface IAverageExchangeRate {
    rate: number,
    statusCode: number,
    message: string
}

async function GetAverageExchangeRate(code: string, date: Date) : Promise<IAverageExchangeRate>{
    let path = `${apiPath}/exchange/average/${code}/${date.getFullYear()}-${date.getMonth()}-${date.getDay()}`;
    const response = await fetch(path);
    if(response.status !== 200) {
        return {rate: 0, statusCode: response.status, message: await response.text()};
    }
    const data = await response.json();
    return {rate: data.exchangeRate, statusCode: response.status, message:""};
}

interface IExtremeExchangeRates {
    minRate: number,
    minDate: Date,
    maxRate: number,
    maxDate: Date,
    statusCode: number,
    message: string
}

async function GetExtremeExchangeRates(code: string, quotations: number) : Promise<IExtremeExchangeRates>{
    let path = `${apiPath}/exchange/extremes/${code}/${quotations}`;
    const response = await fetch(path);
    if(response.status !== 200) {
        return {minRate: 0, minDate: new Date(), maxRate: 0, maxDate: new Date(),
            statusCode: response.status, message: await response.text()};
    }
    const data = await response.json();
    return {minRate: data.minExchangeRate.value, minDate: data.minExchangeRate.date,
        maxRate: data.maxExchangeRate.value, maxDate: data.maxExchangeRate.date,
    statusCode: response.status, message: ""};
}

interface IMaxDifference {
    difference: number,
    date: Date,
    statusCode: number,
    message: string
}

async function GetMaxDifference(code: string, quotations: number) : Promise<IMaxDifference> {
    let path = `${apiPath}/exchange/maxBuyAskDifference/${code}/${quotations}`;
    const response = await fetch(path);
    if(response.status !== 200) {
        return {difference: 0, date: new Date(),
            statusCode: response.status, message: await response.text()};
    }
    const data = await response.json();
    return {difference: data.maxDifference.value, date: data.maxDifference.date,
        statusCode: response.status, message: ""};
}


export {GetAverageExchangeRate, GetExtremeExchangeRates, GetMaxDifference};