import { Card } from "antd"
import { Link } from "react-router-dom"

const Login = () =>{

    

    return(
        <>
            <Card>
                Login <br/>

                <Link to="/register">Register</Link>
            </Card>
        </>
    )
}

export default Login