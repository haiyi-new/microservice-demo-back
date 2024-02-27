import { FusionAuthClient } from '@fusionauth/typescript-client';

const client = new FusionAuthClient('b4p8Uhwx9HNo_KIGSrYKp2qdyy9_BmeF31HSsInBS3ktQq7JWGicFheh', 'http://localhost:9011');

async function getToken() {
    try {
        const response = await client.login({
            loginId: 'client',
            password: '123456789',
            applicationId: 'a92d900d-d460-4baa-b019-149894547bda'
        });

        if (response.wasSuccessful()) {
            console.log('Access Token: ', response.response.token);
        } else {
            console.error('Error getting token: ', response);
        }
    } catch (error) {
        console.error('Error getting token: ', error);
    }
}


async function introspect() {
    const accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjZlNDcyZmRlZSJ9.eyJhdWQiOiJhOTJkOTAwZC1kNDYwLTRiYWEtYjAxOS0xNDk4OTQ1NDdiZGEiLCJleHAiOjE2ODY2NTM5NTgsImlhdCI6MTY4NjY1MDM1OCwiaXNzIjoiYWNtZS5jb20iLCJzdWIiOiJmZmIyMzU3Ni02ODZjLTRkM2EtYTMxMy1iMjUwNWUxMzRlMWYiLCJqdGkiOiJiZDJiYzNlNy02MDA4LTQxODEtODhkYi04NjNjZWNjN2QyZTIiLCJhdXRoZW50aWNhdGlvblR5cGUiOiJQQVNTV09SRCIsImVtYWlsIjoiY2xpZW50QGNsaWVudC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwicHJlZmVycmVkX3VzZXJuYW1lIjoiY2xpZW50IiwiYXV0aF90aW1lIjoxNjg2NjUwMzU4LCJ0aWQiOiI1ZWZkNzc1ZS0xYjlmLWNiZjAtMmQ0OS0xZTI3ZTgyNjg4YWYifQ.T-A9_ckTCmov6xmn7arveimLm1qnCvGSM1-xXuacAeE";

    try {
        const response = await client.introspectAccessToken("a92d900d-d460-4baa-b019-149894547bda", accessToken);

        if (response.wasSuccessful()) {
            console.log(response.response);

        } else {
            console.log(response.response);
        }
    } catch (error) {
        console.error('Error introspecting token: ', error);
        console.log({ error: 'Internal Server Error' });
    }
}

// getToken();
introspect();