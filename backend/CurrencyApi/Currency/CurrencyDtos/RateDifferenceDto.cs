﻿using CurrencyApi.Currency.CurrencyService.Models;

namespace CurrencyApi.Currency.CurrencyDtos;


public class RateDifferenceDto
{
    public string CurrencyCode { get; set; }
    public double Quotations { get; set; }
    public DatedValue MaxDifference { get; set; }
    public RateDifferenceDto(string currencyCode, double quotations, DatedValue maxDifference) 
        => (CurrencyCode, Quotations, MaxDifference) = (currencyCode, quotations, maxDifference);
}