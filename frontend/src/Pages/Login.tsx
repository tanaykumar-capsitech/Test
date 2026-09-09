import { Card } from "antd"
import { Link } from "react-router-dom"

const Login = () =>{

    

    return(
        <>
            <Card style={{ margin: 'auto', marginTop: 300, width: 400 }}>
                Login <br/>

                <Link to="/register">Register</Link>
            </Card>
        </>
    )
}

export default Login