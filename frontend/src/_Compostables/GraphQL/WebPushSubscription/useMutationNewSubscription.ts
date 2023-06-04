import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag';
import type { Ref } from 'vue';
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import type { IWebPushSubscription } from './IWebPushSubscription';

const useMutationNewSubscription = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`
	mutation newSubscription($payload: Input_WebPushSubscriptionDocument!) {
		webPushSubscription {
			newSubscription(payload: $payload) {
				status
			}
		}
	}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationNewSubscription error', error);
	});
	
	const fn = (payload: Ref<IWebPushSubscription>) => {
		
		// console.log('payload', payload);
		
		mutate({
			payload: payload.value,
		});
	};
	
	return {
		newSubscription: fn,
		newSubscriptionLoading: loading,
		newSubscriptionError: onError,
		newSubscriptionDone: onDone,
	}
}

export { useMutationNewSubscription }