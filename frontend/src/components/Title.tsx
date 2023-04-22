interface TitleProps {
    value:string
}

function Title(props: TitleProps){
    return (
        <h2 className="display-5">{props.value}</h2>
    )
}

export {Title};