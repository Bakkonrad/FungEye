import axios from 'axios';
import { jwtDecode } from "jwt-decode";
import { isLoggedIn } from './AuthService';

const $http = axios.create({
    baseURL: "http://localhost:5268/",
    headers: {
        "Content-type": "application/json",
        "Authorization": "Bearer " + localStorage.getItem('token')
    }
});

const getAllUsers = async (page, search) => {
    try {
        const userId = parseInt(localStorage.getItem("id"));
        const formData = new FormData();
        formData.append('userId', userId);
        if (page) {
            formData.append('page', page);
        }
        if (search) {
            formData.append('search', search);
        }
        const response = await $http.post('api/User/getUsers', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        if (response.status === 200) {
            return response;
        }
        else {
            alert("Error loading data");
            return [];
        }
        // if (page === 1) {
        //     return [{
        //         username: "MPudzianowski",
        //         email: "mpudzianowski@gmail.com",
        //         firstName: "Mariusz",
        //         lastName: "Pudzianowski",
        //     },
        //     {
        //         username: "RLewandowski",
        //         email: "rlewandowski@gmail.com",
        //         firstName: "Robert",
        //         lastName: "Lewandowski",
        //     },
        //     {
        //         username: "AMałysz",
        //         email: "amalysz@gmail.com",
        //         firstName: "Adam",
        //         lastName: "Małysz",
        //     },
        //     {
        //         username: "ATest",
        //         email: "atest@gmail.com",
        //         firstName: "Adam",
        //         lastName: "Test",
        //     },];
        // }
        // else if (page === 2) {
        //     return [
        //         {
        //             username: "JSmith",
        //             email: "jsmith@gmail.com",
        //             firstName: "John",
        //             lastName: "Smith",
        //         },
        //         {
        //             username: "ALee",
        //             email: "alee@gmail.com",
        //             firstName: "Alice",
        //             lastName: "Lee",
        //         },
        //         {
        //             username: "MMiller",
        //             email: "mmiller@gmail.com",
        //             firstName: "Michael",
        //             lastName: "Miller",
        //         },
        //         {
        //             username: "SKim",
        //             email: "skim@gmail.com",
        //             firstName: "Sara",
        //             lastName: "Kim",
        //         },
        //         {
        //             username: "DBrown",
        //             email: "dbrown@gmail.com",
        //             firstName: "David",
        //             lastName: "Brown",
        //         },
        //         {
        //             username: "ELopez",
        //             email: "elopez@gmail.com",
        //             firstName: "Emma",
        //             lastName: "Lopez",
        //         },
        //         {
        //             username: "RClark",
        //             email: "rclark@gmail.com",
        //             firstName: "Ryan",
        //             lastName: "Clark",
        //         },
        //         {
        //             username: "KWilson",
        //             email: "kwilson@gmail.com",
        //             firstName: "Karen",
        //             lastName: "Wilson",
        //         },
        //         {
        //             username: "JAnderson",
        //             email: "janderson@gmail.com",
        //             firstName: "James",
        //             lastName: "Anderson",
        //         },
        //         {
        //             username: "SLee",
        //             email: "slee@gmail.com",
        //             firstName: "Sophia",
        //             lastName: "Lee",
        //         },
        //         {
        //             username: "TThompson",
        //             email: "tthompson@gmail.com",
        //             firstName: "Thomas",
        //             lastName: "Thompson",
        //         },
        //         {
        //             username: "JHarris",
        //             email: "jharris@gmail.com",
        //             firstName: "Jessica",
        //             lastName: "Harris",
        //         },
        //         {
        //             username: "AMartin",
        //             email: "amartin@gmail.com",
        //             firstName: "Andrew",
        //             lastName: "Martin",
        //         },
        //         {
        //             username: "JWalker",
        //             email: "jwalker@gmail.com",
        //             firstName: "Jennifer",
        //             lastName: "Walker",
        //         },
        //         {
        //             username: "JGarcia",
        //             email: "jgarcia@gmail.com",
        //             firstName: "Joseph",
        //             lastName: "Garcia",
        //         },
        //         {
        //             username: "KRobinson",
        //             email: "krobinson@gmail.com",
        //             firstName: "Kimberly",
        //             lastName: "Robinson",
        //         },
        //         {
        //             username: "JWright",
        //             email: "jwright@gmail.com",
        //             firstName: "Jonathan",
        //             lastName: "Wright",
        //         },
        //         {
        //             username: "MYoung",
        //             email: "myoung@gmail.com",
        //             firstName: "Melissa",
        //             lastName: "Young",
        //         },
        //         {
        //             username: "JHall",
        //             email: "jhall@gmail.com",
        //             firstName: "Jason",
        //             lastName: "Hall",
        //         },
        //         {
        //             username: "JAllen",
        //             email: "jallen@gmail.com",
        //             firstName: "Julia",
        //             lastName: "Allen",
        //         },
        //         {
        //             username: "JKing",
        //             email: "jking@gmail.com",
        //             firstName: "Justin",
        //             lastName: "King",
        //         },
        //         {
        //             username: "JScott",
        //             email: "jscott@gmail.com",
        //             firstName: "Jessica",
        //             lastName: "Scott",
        //         },
        //         {
        //             username: "JGreen",
        //             email: "jgreen@gmail.com",
        //             firstName: "Jeremy",
        //             lastName: "Green",
        //         },
        //         {
        //             username: "JAdams",
        //             email: "jadams@gmail.com",
        //             firstName: "Julie",
        //             lastName: "Adams",
        //         },
        //         {
        //             username: "JHill",
        //             email: "jhill@gmail.com",
        //             firstName: "Jacob",
        //             lastName: "Hill",
        //         },
        //         {
        //             username: "JTurner",
        //             email: "jturner@gmail.com",
        //             firstName: "Jennifer",
        //             lastName: "Turner",
        //         },
        //         {
        //             username: "JCollins",
        //             email: "jcollins@gmail.com",
        //             firstName: "Joshua",
        //             lastName: "Collins",
        //         },
        //         {
        //             username: "JStewart",
        //             email: "jstewart@gmail.com",
        //             firstName: "Jessica",
        //             lastName: "Stewart",
        //         },
        //         {
        //             username: "JMorris",
        //             email: "jmorris@gmail.com",
        //             firstName: "John",
        //             lastName: "Morris",
        //         }];
        // }
    } catch (error) {
        console.error('Error loading data:', error);
        return [];
    }
}

const deleteUser = async (id) => {
    try {
        const response = await $http.delete('api/Admin/deleteUser/' + id);
        if (response.status === 200) {
            return true;
        }
        return false;
    } catch (error) {
        console.error('Error loading data:', error);
        return false;
    }
}

const updateUser = async (user) => {
    try {
        const response = await $http.put('api/Admin/updateUser', user);
        if (response.status === 200) {
            return true;
        }
        return false;
    } catch (error) {
        console.error('Error loading data:', error);
        return false;
    }
}

const getUser = async (id) => {
    try {
        const response = await $http.get('api/Admin/getUser/' + id);
        if (response.status === 200) {
            return response.data;
        }
        return [];
    } catch (error) {
        console.error('Error loading data:', error);
        return [];
    }
}

export default {
    getAllUsers,
    deleteUser,
    updateUser,
    getUser
}