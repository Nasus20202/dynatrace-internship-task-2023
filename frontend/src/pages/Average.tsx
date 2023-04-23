import {Title} from "../components/Title";
import GetAverageExchangeRate from "../api/AverageExchangeRate";
import React from "react";
import {Form, Button, Row, Col, Alert} from "react-bootstrap";

interface IProps {
}

interface IState {
    currencyCode: string;
    currencyCodeInput: string;
    date: string;
    dateInput: string;
    result: number;
    waiting: boolean;
    message: string;
}

class Average extends React.Component<IProps, IState> {
    constructor(props: IProps) {
        super(props);
        this.state = {
            currencyCode: "",
            currencyCodeInput: "",
            date: "",
            dateInput: "",
            result: -1,
            waiting: false,
            message: "",
        };

        this.handleCurrencyCodeChange = this.handleCurrencyCodeChange.bind(this);
        this.handleDateChange = this.handleDateChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleCurrencyCodeChange = (event: any) => {
        this.setState({
            currencyCodeInput: event.target.value.toUpperCase(),
        });
    }

    handleDateChange = (event: any) => {
        this.setState({
            dateInput: event.target.value
        });
    }

    handleSubmit = (event: any) => {
        event.preventDefault();
        if(!this.validateForm())
            return;
        this.setState({
            waiting: true,
            currencyCode: this.state.currencyCodeInput,
            date: this.state.dateInput
        });
        const date = new Date(this.state.dateInput);
        GetAverageExchangeRate(this.state.currencyCodeInput, date).then(response => {
            this.setState({
                waiting: false,
                result: response.rate !== undefined ? response.rate as number : -1,
                message: response.message !== undefined ? response.message as string : ""
            });
        });
    }

    validateForm() : boolean{
        if(this.state.currencyCodeInput.length !== 3) {
            this.setState({
                message: "Currency code must be 3 characters long"
            });
            return false;
        }
        if(this.state.dateInput.length === 0) {
            this.setState({
                message: "Date must be specified"
            });
            return false;
        }
        this.setState({
            message: ""
        })
        return true;
    }

    render() {
        return (
        <div>
            <Title value="Average"/>
            <Form onSubmit={this.handleSubmit}>
                <Form.Group as={Row} className="mb-3">
                    <Form.Label column sm="2">
                        Currency code:
                    </Form.Label>
                    <Col sm="10">
                        <Form.Control type="text" placeholder="Enter currency code" name="currencyCode" onChange={this.handleCurrencyCodeChange} disabled={this.state.waiting}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row} className="mb-3">
                    <Form.Label column sm="2">
                        Date:
                    </Form.Label>
                    <Col sm="10">
                        <Form.Control type="date" placeholder="Enter date" name="date" onChange={this.handleDateChange} disabled={this.state.waiting}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row} className="mb-3">
                    <Col sm={{span: 10, offset: 2}}>
                        <Button type="submit" disabled={this.state.waiting}>Submit</Button>
                    </Col>
                </Form.Group>
            </Form>
            <Alert variant="danger" show={this.state.message.length !== 0}>{this.state.message}</Alert>
            <Alert variant="success" show={this.state.result !== -1}>Average exchange rate for {this.state.currencyCode} on {this.state.date} is {this.state.result} zł</Alert>
        </div>
        );
    }
}

export {Average};