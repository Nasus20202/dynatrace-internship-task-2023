import apiPath from './ApiPath'

type AverageExchangeRate = {
    rate?: number,
    statusCode: number,
    message?: string
}

export default async function GetAverageExchangeRate(code: string, date: Date) : Promise<AverageExchangeRate>{
    const path = `${apiPath}/exchange/average/${code}/${date.getFullYear()}-${date.getMonth()+1}-${date.getDate()}`;
    const response = await fetch(path);
    const data = await response.json();
    if(response.status !== 200)
        return {statusCode: response.status, message: data.message};
    return {rate: data.exchangeRate, statusCode: response.status};
}


